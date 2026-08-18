import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const alertVariants = cva('rounded-md border p-4 text-left', {
  variants: {
    tone: {
      info: 'border-[var(--accent-border)] bg-[var(--accent-bg)] text-[var(--text-h)]',
      warning:
        'border-amber-300 bg-amber-50 text-amber-900 dark:border-amber-700 dark:bg-amber-950/30 dark:text-amber-100',
      error:
        'border-red-300 bg-red-50 text-red-900 dark:border-red-700 dark:bg-red-950/30 dark:text-red-100',
    },
  },
  defaultVariants: {
    tone: 'info',
  },
})

type AlertProps = HTMLAttributes<HTMLDivElement> &
  VariantProps<typeof alertVariants> & {
    title?: string
    children: ReactNode
  }

export default function Alert({
  tone,
  title,
  children,
  className,
  ...props
}: AlertProps) {
  return (
    <div
      role="alert"
      className={cn(alertVariants({ tone }), className)}
      {...props}
    >
      {title && <p className="mb-1 font-medium">{title}</p>}
      <div className="text-sm">{children}</div>
    </div>
  )
}
