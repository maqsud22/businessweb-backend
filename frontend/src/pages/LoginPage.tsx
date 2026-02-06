import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { apiClient } from '../api/client';
import { storage } from '../lib/storage';

const schema = z.object({
  username: z.string().min(3),
  password: z.string().min(4)
});

type FormValues = z.infer<typeof schema>;

export default function LoginPage() {
  const [error, setError] = useState<string | null>(null);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<FormValues>({ resolver: zodResolver(schema) });

  const onSubmit = async (values: FormValues) => {
    setError(null);
    try {
      const response = await apiClient.post('/api/Auth/login', values);
      const token = response.data?.token;
      if (!token) {
        setError('Token not returned from API');
        return;
      }
      storage.setToken(token);
      window.location.href = '/';
    } catch (err: any) {
      setError(err?.response?.data?.title || 'Login failed');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100">
      <div className="card w-full max-w-sm">
        <h1 className="mb-2 text-xl font-semibold">Admin Login</h1>
        <p className="mb-4 text-sm text-slate-500">Use your admin credentials.</p>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm">Username</label>
            <input className="input" {...register('username')} />
            {errors.username ? (
              <div className="mt-1 text-xs text-red-600">{errors.username.message}</div>
            ) : null}
          </div>
          <div>
            <label className="mb-1 block text-sm">Password</label>
            <input type="password" className="input" {...register('password')} />
            {errors.password ? (
              <div className="mt-1 text-xs text-red-600">{errors.password.message}</div>
            ) : null}
          </div>
          {error ? <div className="text-sm text-red-600">{error}</div> : null}
          <button className="button w-full" disabled={isSubmitting}>
            {isSubmitting ? 'Signing in...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
  );
}
