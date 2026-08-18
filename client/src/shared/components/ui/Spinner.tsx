import { cva, type VariantProps } from 'class-variance-authority'
import { cn } from '@/lib/cn'

export const spinnerVariants = cva(
  'animate-spin rounded-full border-2 border-[var(--border)] border-t-[var(--accent)]',
  {
    variants: {
      size: {
        sm: 'size-4',
        md: 'size-6',
        lg: 'size-8',
      },
    },
    defaultVariants: {
      size: 'md',
    },
  },
)

type SpinnerProps = VariantProps<typeof spinnerVariants> & {
  className?: string
}

export default function Spinner({ size, className }: SpinnerProps) {
  return (
    <div
      role="status"
      aria-label="Loading"
      className={cn(spinnerVariants({ size }), className)}
    />
  )
}
