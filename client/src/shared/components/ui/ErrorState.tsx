import { cva } from 'class-variance-authority'
import { cn } from '@/lib/cn'

export const errorStateVariants = cva(
  'rounded-md border border-red-300 bg-red-50 p-4 text-left dark:border-red-700 dark:bg-red-950/30',
)

type ErrorStateProps = {
  title?: string
  message: string
  className?: string
}

export default function ErrorState({
  title = 'Something went wrong',
  message,
  className,
}: ErrorStateProps) {
  return (
    <div className={cn(errorStateVariants(), className)}>
      <p className="font-medium text-red-700 dark:text-red-300">{title}</p>
      <p className="text-sm text-red-600 dark:text-red-400">{message}</p>
    </div>
  )
}
