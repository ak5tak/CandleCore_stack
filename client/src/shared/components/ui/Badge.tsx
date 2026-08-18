import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const badgeVariants = cva(
  'inline-flex rounded px-2 py-0.5 text-xs font-medium',
  {
    variants: {
      tone: {
        neutral: 'bg-[var(--code-bg)] text-[var(--text-h)]',
        success:
          'bg-green-100 text-green-800 dark:bg-green-900/40 dark:text-green-100',
        danger:
          'bg-red-100 text-red-800 dark:bg-red-900/40 dark:text-red-100',
        accent:
          'border border-[var(--accent-border)] bg-[var(--accent-bg)] text-[var(--accent)]',
      },
    },
    defaultVariants: {
      tone: 'neutral',
    },
  },
)

type BadgeProps = HTMLAttributes<HTMLSpanElement> &
  VariantProps<typeof badgeVariants> & {
    children: ReactNode
  }

export default function Badge({
  children,
  tone,
  className,
  ...props
}: BadgeProps) {
  return (
    <span className={cn(badgeVariants({ tone }), className)} {...props}>
      {children}
    </span>
  )
}
