import { cva, type VariantProps } from 'class-variance-authority'
import type { HTMLAttributes, ReactNode } from 'react'
import { cn } from '@/lib/cn'

export const textVariants = cva('', {
  variants: {
    size: {
      sm: 'text-sm',
      base: 'text-base',
      lg: 'text-lg',
    },
    muted: {
      true: 'text-[var(--text)]',
      false: 'text-[var(--text-h)]',
    },
  },
  defaultVariants: {
    size: 'base',
    muted: false,
  },
})

type TextProps = HTMLAttributes<HTMLParagraphElement> &
  VariantProps<typeof textVariants> & {
    children: ReactNode
  }

export default function Text({
  children,
  size,
  muted,
  className,
  ...props
}: TextProps) {
  return (
    <p className={cn(textVariants({ size, muted }), className)} {...props}>
      {children}
    </p>
  )
}
