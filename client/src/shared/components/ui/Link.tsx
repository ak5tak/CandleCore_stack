import { Slot } from '@radix-ui/react-slot'
import {
  Link as TanStackLink,
  type LinkComponentProps,
} from '@tanstack/react-router'
import { cva, type VariantProps } from 'class-variance-authority'
import type { ComponentProps } from 'react'
import { cn } from '@/lib/cn'

export const linkVariants = cva(
  'inline-flex items-center gap-2 font-medium transition',
  {
    variants: {
      variant: {
        default: 'text-[var(--text-h)] hover:underline',
        secondary: 'text-[var(--text)] hover:text-[var(--text-h)]',
        ghost:
          'rounded-md hover:bg-[var(--accent-bg)] hover:no-underline',
      },
    },
    defaultVariants: {
      variant: 'ghost',
    },
  },
)

type LinkProps = LinkComponentProps<'a'> &
  VariantProps<typeof linkVariants> & {
    asChild?: boolean
  }

export default function Link({
  className,
  variant,
  asChild = false,
  ...props
}: LinkProps) {
  const classes = cn(linkVariants({ variant }), className)

  if (asChild) {
    return <Slot className={classes} {...(props as ComponentProps<typeof Slot>)} />
  }

  return <TanStackLink className={classes} {...props} />
}
