"use client";

import {
  createContext, useContext, useId, type ReactNode,
  type InputHTMLAttributes, type SelectHTMLAttributes, type TextareaHTMLAttributes,
} from "react";

/* Accessible field primitive.
   Wires label↔control association (WCAG 1.3.1 / 3.3.2 / 4.1.2) and announces
   required state + validation errors (3.3.1) via aria-required / aria-invalid /
   aria-describedby, with errors in role="alert". Visual markup is unchanged. */

interface FieldInfo {
  id?: string;
  describedBy?: string;
  invalid?: boolean;
  required?: boolean;
}
const FieldCtx = createContext<FieldInfo>({});
export function useField(): FieldInfo {
  return useContext(FieldCtx);
}

export function Field({
  label,
  required,
  error,
  hint,
  children,
  className,
  style,
}: {
  label: ReactNode;
  required?: boolean;
  error?: string;
  hint?: ReactNode;
  children: ReactNode;
  className?: string;
  style?: React.CSSProperties;
}) {
  const base = useId();
  const id = `${base}-c`;
  const errId = `${base}-e`;
  const hintId = `${base}-h`;
  const hasError = !!error;
  const describedBy =
    [hint ? hintId : null, hasError ? errId : null].filter(Boolean).join(" ") || undefined;

  return (
    <FieldCtx.Provider value={{ id, describedBy, invalid: hasError, required }}>
      <div className={`field${className ? " " + className : ""}`} style={style}>
        <label className="label" htmlFor={id}>
          {label}
          {required && <span className="req" aria-hidden>*</span>}
        </label>
        {children}
        {hint && !hasError && <span className="field-hint" id={hintId}>{hint}</span>}
        {hasError && <span className="field-error" id={errId} role="alert">{error}</span>}
      </div>
    </FieldCtx.Provider>
  );
}

/** Bare Field for controls that render their own wrapper (e.g. a row of inputs).
 *  Provides the a11y context without the .field div. */
export function FieldGroup({
  required,
  error,
  hint,
  children,
}: {
  required?: boolean;
  error?: string;
  hint?: ReactNode;
  children: (info: FieldInfo) => ReactNode;
}) {
  const base = useId();
  const id = `${base}-c`;
  const hasError = !!error;
  const describedBy = hasError ? `${base}-e` : hint ? `${base}-h` : undefined;
  return <>{children({ id, describedBy, invalid: hasError, required })}</>;
}

export function Input(props: InputHTMLAttributes<HTMLInputElement>) {
  const f = useField();
  const { className, ...rest } = props;
  return (
    <input
      id={f.id}
      aria-describedby={f.describedBy}
      aria-invalid={f.invalid || undefined}
      aria-required={f.required || undefined}
      className={`input${f.invalid ? " input-error" : ""}${className ? " " + className : ""}`}
      {...rest}
    />
  );
}

export function Select(props: SelectHTMLAttributes<HTMLSelectElement>) {
  const f = useField();
  const { className, children, ...rest } = props;
  return (
    <select
      id={f.id}
      aria-describedby={f.describedBy}
      aria-invalid={f.invalid || undefined}
      aria-required={f.required || undefined}
      className={`select${f.invalid ? " input-error" : ""}${className ? " " + className : ""}`}
      {...rest}
    >
      {children}
    </select>
  );
}

export function Textarea(props: TextareaHTMLAttributes<HTMLTextAreaElement>) {
  const f = useField();
  const { className, ...rest } = props;
  return (
    <textarea
      id={f.id}
      aria-describedby={f.describedBy}
      aria-invalid={f.invalid || undefined}
      aria-required={f.required || undefined}
      className={`textarea${f.invalid ? " input-error" : ""}${className ? " " + className : ""}`}
      {...rest}
    />
  );
}
