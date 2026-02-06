import { useEffect, useState } from 'react';
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

  useEffect(() => {
    document.body.classList.add('login-theme');
    return () => {
      document.body.classList.remove('login-theme');
    };
  }, []);

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
    <div className="login-page">
      <div className="wrapper w-full max-w-sm">
        <div className="mb-6 text-center">
          <h1 className="text-2xl font-semibold text-white">BusinessWeb Admin</h1>
          <p className="mt-2 text-sm text-white/80">Kirish uchun admin ma'lumotlaringizni kiriting.</p>
        </div>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="input-box">
            <input placeholder="Username" {...register('username')} />
          </div>
          {errors.username ? (
            <div className="mb-2 text-xs text-red-200">{errors.username.message}</div>
          ) : null}

          <div className="input-box">
            <input type="password" placeholder="Password" {...register('password')} />
          </div>
          {errors.password ? (
            <div className="mb-2 text-xs text-red-200">{errors.password.message}</div>
          ) : null}

          {error ? <div className="mb-3 text-sm text-red-200">{error}</div> : null}

          <button className="btn" disabled={isSubmitting}>
            {isSubmitting ? 'Signing in...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
  );
}
