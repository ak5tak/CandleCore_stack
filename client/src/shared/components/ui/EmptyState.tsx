import type { ReactNode } from 'react'
import { cn } from '@/lib/cn'

type EmptyStateProps = {
  title: string
  description?: string
  action?: ReactNode
  className?: string
}

export default function EmptyState({
  title,
  description,
  action,
  className,
}: EmptyStateProps) {
  return (
    <div
      className={cn(
        'flex flex-col items-center gap-2 py-8 text-center',
        className,
      )}
    >
      <p className="font-medium text-[var(--text-h)]">{title}</p>
      {description && (
        <p className="text-sm text-[var(--text)]">{description}</p>
      )}
      {action}
    </div>
  )
}
