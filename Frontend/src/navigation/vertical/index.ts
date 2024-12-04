// ** Type import
import { VerticalNavItemsType } from 'src/@core/layouts/types'

const navigation = (): VerticalNavItemsType => {
  return [
    {
      title: 'Users',
      icon: 'tabler:users',
      path: '/user/list',
      action: 'read',
      subject: 'user-management'
    },
    {
      title: 'Account Logs',
      icon: 'tabler:currency-naira',
      path: '/account-log/list',
      action: 'read',
      subject: 'account-log',
    },
    {
      title: 'Fund Wallet Logs',
      icon: 'tabler:currency-naira',
      path: '/fund-log/list',
      action: 'read',
      subject: 'fund-log',
    },
    {
      title: 'Withdraw Logs',
      icon: 'tabler:currency-naira',
      path: '/withdraw-log/list',
      action: 'read',
      subject: 'withdraw-log',
    },
  ]
}

export default navigation
