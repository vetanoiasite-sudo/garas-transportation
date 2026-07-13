"use client";

import { IconInbox } from "./Icons";

export function EmptyState({ message, action }: { message: string; action?: React.ReactNode }) {
  return (
    <div className="state-box">
      <IconInbox />
      <p>{message}</p>
      {action}
    </div>
  );
}

export function LoadingState() {
  return (
    <div className="state-box">
      <span className="spinner spinner-brand" style={{ width: 28, height: 28 }} />
    </div>
  );
}
