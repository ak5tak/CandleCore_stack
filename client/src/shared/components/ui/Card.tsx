import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const cardVariants = cva(
  'rounded-lg border border-[var(--border)] bg-[var(--bg)] shadow-[var(--shadow)]',
  {
    variants: {
      padding: {
        none: '',
        sm: 'p-3',
        md: 'p-4',
      },
    },
    defaultVariants: {
      padding: 'md',
    },
  },
)

type CardProps = HTMLAttributes<HTMLDivElement> &
  VariantProps<typeof cardVariants> & {
    children: ReactNode
  }

export default function Card({
  children,
  padding,
  className,
  ...props
}: CardProps) {
  return (
    <div className={cn(cardVariants({ padding }), className)} {...props}>
      {children}
    </div>
  )
}
