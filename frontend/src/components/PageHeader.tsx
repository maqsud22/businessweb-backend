import { ReactNode } from 'react';

interface Props {
  title: string;
  actions?: ReactNode;
  subtitle?: string;
}

export default function PageHeader({ title, actions, subtitle }: Props) {
  return (
    <div className="mb-4 flex items-center justify-between">
      <div>
        <h1 className="text-xl font-semibold text-slate-900">{title}</h1>
        {subtitle ? <p className="text-sm text-slate-500">{subtitle}</p> : null}
      </div>
      {actions}
    </div>
  );
}
