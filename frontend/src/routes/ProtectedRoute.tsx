import { Navigate } from 'react-router-dom';
import { storage } from '../lib/storage';

interface Props {
  children: JSX.Element;
}

export default function ProtectedRoute({ children }: Props) {
  const token = storage.getToken();
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return children;
}
