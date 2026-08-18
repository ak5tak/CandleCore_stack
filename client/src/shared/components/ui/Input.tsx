import { cva } from 'class-variance-authority'
import type { InputHTMLAttributes } from 'react'
import { cn } from '@/lib/cn'

export const inputVariants = cva(
  'rounded-md border border-[var(--border)] bg-[var(--bg)] px-3 py-2 text-[var(--text-h)] outline-none focus:border-[var(--accent-border)]',
  {
    variants: {
      error: {
        true: 'border-red-500',
        false: '',
      },
    },
    defaultVariants: {
      error: false,
    },
  },
)

type InputProps = InputHTMLAttributes<HTMLInputElement> & {
  label?: string
  error?: string
}

export default function Input({
  label,
  error,
  className,
  id,
  ...props
}: InputProps) {
  const inputId = id ?? label?.toLowerCase().replace(/\s+/g, '-')

  return (
    <label className="flex flex-col gap-1 text-left">
      {label && (
        <span className="text-sm text-[var(--text-h)]">{label}</span>
      )}
      <input
        id={inputId}
        className={cn(inputVariants({ error: Boolean(error) }), className)}
        {...props}
      />
      {error && <span className="text-xs text-red-500">{error}</span>}
    </label>
  )
}
