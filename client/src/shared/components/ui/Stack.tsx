import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const stackVariants = cva('flex', {
  variants: {
    direction: {
      row: 'flex-row',
      col: 'flex-col',
    },
    gap: {
      1: 'gap-1',
      2: 'gap-2',
      3: 'gap-3',
      4: 'gap-4',
      6: 'gap-6',
      8: 'gap-8',
    },
  },
  defaultVariants: {
    direction: 'col',
    gap: 4,
  },
})

type StackProps = HTMLAttributes<HTMLDivElement> &
  VariantProps<typeof stackVariants> & {
    children: ReactNode
  }

export default function Stack({
  children,
  direction,
  gap,
  className,
  ...props
}: StackProps) {
  return (
    <div className={cn(stackVariants({ direction, gap }), className)} {...props}>
      {children}
    </div>
  )
}
