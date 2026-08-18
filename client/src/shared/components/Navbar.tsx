import { useState } from 'react'
import {
  LayoutDashboard,
  LineChart,
  BarChart3,
  Menu,
  X,
} from 'lucide-react'
import { DEFAULT_PERIOD } from '@/shared/constants/defaults'
import Link from './ui/Link'
import Button from './ui/Button'
import { ThemeToggle } from './ThemeToggle'


export default function Navbar() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)

  const navLinkClassName =
    'rounded-lg p-2 text-lg hover:bg-slate-100 dark:hover:bg-slate-800'

  const activeNavLinkClassName = 'bg-slate-100 dark:bg-slate-800'

  return (
    <>
      <nav className="w-full py-3 dark:border-neutral-800">
        <div className="grid w-full grid-cols-[1fr_auto_1fr] items-end gap-4">
          <p className="m-0 whitespace-nowrap text-xl leading-none font-semibold text-[var(--text-h)]">
            Bitcoin Analyzer
          </p>

          <div className="hidden flex-nowrap items-center gap-2 whitespace-nowrap justify-self-center md:flex">
            <Link
              to="/"
              search={{ period: DEFAULT_PERIOD }}
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
            >
              Dashboard<LayoutDashboard className="h-5 w-5" />
            </Link>

            <Link
              to="/charts"
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
            >
              Charts<LineChart className="h-5 w-5" />
            </Link>

            <Link
              to="/analysis"
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
            >
              Analysis<BarChart3 className="h-5 w-5" />
            </Link>
          </div>

          <div className="flex items-center justify-self-end gap-3 whitespace-nowrap">
            <Button
              variant="ghost"
              onClick={() => setMobileMenuOpen(true)}
              className="p-2 md:hidden"
            >
              <Menu className="h-5 w-5" />
            </Button>

            <div className="hidden items-center gap-3 md:flex">
              <ThemeToggle />
            </div>
          </div>
        </div>
      </nav>

      {mobileMenuOpen && (
        <div className="fixed inset-0 z-50 bg-white dark:bg-neutral-950 md:hidden">
          <div className="flex items-center justify-between border-b border-slate-200 px-4 py-3 dark:border-neutral-800">
            <p className="m-0 whitespace-nowrap text-xl leading-none font-semibold text-[var(--text-h)]">
              Bitcoin Analyzer
            </p>

            <Button
              variant="ghost"
              onClick={() => setMobileMenuOpen(false)}
              className="p-2"
            >
              <X className="h-5 w-5" />
            </Button>
          </div>

          <div className="flex flex-col gap-2 p-4">
            <Link
              to="/"
              search={{ period: DEFAULT_PERIOD }}
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
              onClick={() => setMobileMenuOpen(false)}
            >
              <span className="flex items-center gap-3">
                <LayoutDashboard className="h-5 w-5" />
                Dashboard
              </span>
            </Link>

            <Link
              to="/charts"
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
              onClick={() => setMobileMenuOpen(false)}
            >
              <span className="flex items-center gap-3">
                <LineChart className="h-5 w-5" />
                Charts
              </span>
            </Link>

            <Link
              to="/analysis"
              variant="ghost"
              className={navLinkClassName}
              activeProps={{ className: activeNavLinkClassName }}
              onClick={() => setMobileMenuOpen(false)}
            >
              <span className="flex items-center gap-3">
                <BarChart3 className="h-5 w-5" />
                Analysis
              </span>
            </Link>

            <div className="pt-2">
              <ThemeToggle mobile />
            </div>
          </div>
        </div>
      )}
    </>
  )
}
