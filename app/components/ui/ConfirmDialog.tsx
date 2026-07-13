"use client";

import Dialog from "./Dialog";
import { useLocale } from "@/contexts/LocaleContext";

export default function ConfirmDialog({
  title,
  message,
  confirmLabel,
  tone = "danger",
  onConfirm,
  onClose,
}: {
  title: string;
  message: React.ReactNode;
  confirmLabel?: string;
  tone?: "danger" | "success" | "brand";
  onConfirm: () => void;
  onClose: () => void;
}) {
  const { t } = useLocale();
  const btnClass = tone === "success" ? "btn-success" : tone === "brand" ? "btn-brand" : "btn-danger";
  return (
    <Dialog
      title={title}
      onClose={onClose}
      footer={
        <>
          <button className="btn btn-secondary" onClick={onClose}>
            {t("action.cancel")}
          </button>
          <button
            className={`btn ${btnClass}`}
            onClick={() => {
              onConfirm();
              onClose();
            }}
          >
            {confirmLabel ?? t("action.confirm")}
          </button>
        </>
      }
    >
      <p style={{ color: "var(--text-body)", lineHeight: "var(--leading-6)" }}>{message}</p>
    </Dialog>
  );
}
