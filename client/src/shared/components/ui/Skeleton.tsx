import { cn } from '@/lib/cn'

type SkeletonProps = {
  className?: string
}

export default function Skeleton({ className }: SkeletonProps) {
  return (
    <div
      aria-hidden
      className={cn('animate-pulse rounded-md bg-[var(--code-bg)]', className)}
    />
  )
}
