import { Moon, Sun } from 'lucide-react'
import { useTheme } from './ThemeProvider'
import Button from './ui/Button'
import { cn } from '@/lib/cn'

type ThemeToggleProps = {
  mobile?: boolean
}

export function ThemeToggle({ mobile = false }: ThemeToggleProps) {
  const { theme, setTheme } = useTheme()

  const isDark =
    theme === 'dark' ||
    (theme === 'system' &&
      window.matchMedia('(prefers-color-scheme: dark)').matches)

  return (
    <Button
      variant="ghost"
      size="sm"
      className={cn('rounded-lg p-2', mobile && 'w-full justify-start')}
      onClick={() => setTheme(isDark ? 'light' : 'dark')}
      aria-label={isDark ? 'Switch to light mode' : 'Switch to dark mode'}
    >
      {isDark ? (
        <>
          <Sun className="h-5 w-5" />
          {mobile && 'Light Mode'}
        </>
      ) : (
        <>
          <Moon className="h-5 w-5" />
          {mobile && 'Dark Mode'}
        </>
      )}
    </Button>
  )
}
