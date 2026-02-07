interface StatusProps {
  loading?: boolean;
  error?: string | null;
}

export default function Status({ loading, error }: StatusProps) {
  if (loading) {
    return <div className="text-sm text-slate-500">Loading...</div>;
  }
  if (error) {
    return <div className="text-sm text-red-600">{error}</div>;
  }
  return null;
}
