import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

type HeadingLevel = 1 | 2 | 3 | 4

export const headingVariants = cva('font-medium text-[var(--text-h)]', {
  variants: {
    level: {
      1: 'text-3xl tracking-tight sm:text-4xl',
      2: 'text-2xl tracking-tight',
      3: 'text-xl',
      4: 'text-lg',
    },
  },
  defaultVariants: {
    level: 2,
  },
})

const tags: Record<HeadingLevel, 'h1' | 'h2' | 'h3' | 'h4'> = {
  1: 'h1',
  2: 'h2',
  3: 'h3',
  4: 'h4',
}

type HeadingProps = HTMLAttributes<HTMLHeadingElement> &
  Omit<VariantProps<typeof headingVariants>, 'level'> & {
    as?: HeadingLevel
    children: ReactNode
  }

export default function Heading({
  as = 2,
  children,
  className,
  ...props
}: HeadingProps) {
  const Tag = tags[as]

  return (
    <Tag className={cn(headingVariants({ level: as }), className)} {...props}>
      {children}
    </Tag>
  )
}
