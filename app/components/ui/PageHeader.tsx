"use client";

export default function PageHeader({
  title,
  count,
  children,
}: {
  title: string;
  count?: number;
  children?: React.ReactNode;
}) {
  return (
    <div className="row-between wrap mb-4">
      <div className="row" style={{ gap: "var(--space-2)" }}>
        <h1 className="page-title">{title}</h1>
        {count !== undefined && (
          <span className="badge badge-gray" style={{ fontSize: "var(--text-sm)" }}>{count}</span>
        )}
      </div>
      {children && <div className="header-actions">{children}</div>}
    </div>
  );
}
