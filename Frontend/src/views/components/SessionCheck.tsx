import { useEffect } from 'react'
import { isBefore, isDate, parseISO } from 'date-fns'
import { useAuth } from 'src/hooks/useAuth'

// ** Config
import generalConfig from 'src/configs/general'
import { AuthType } from 'src/context/types'

const SessionCheck = () => {
  const auth = useAuth()

  const checkSessionExpiration = () => {
    if (auth.user) {
      try {
        const authTokenString = window.localStorage.getItem(generalConfig.authTokenKeyName)
        const authToken = JSON.parse(authTokenString || '{}') as AuthType
        let today = new Date()
        const isExpired = isBefore(parseISO(authToken.expiresAt as string), today)

        // Check if the user is authenticated and the current time is before the expiry time
        if (isExpired) {
          auth.logout()
        }
      } catch (error) {
        auth.logout()
      }
    }
  }

  useEffect(() => {
    let myInterval = setInterval(checkSessionExpiration, 5000) // Check every 5 seconds
    return () => clearInterval(myInterval)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [auth.user])

  return null
}

export default SessionCheck
