import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const gridVariants = cva('grid gap-4', {
  variants: {
    cols: {
      2: 'grid-cols-1 sm:grid-cols-2',
      3: 'grid-cols-1 sm:grid-cols-2 lg:grid-cols-3',
      4: 'grid-cols-2 lg:grid-cols-4',
    },
  },
  defaultVariants: {
    cols: 3,
  },
})

type GridProps = HTMLAttributes<HTMLDivElement> &
  VariantProps<typeof gridVariants> & {
    children: ReactNode
  }

export default function Grid({ children, cols, className, ...props }: GridProps) {
  return (
    <div className={cn(gridVariants({ cols }), className)} {...props}>
      {children}
    </div>
  )
}
