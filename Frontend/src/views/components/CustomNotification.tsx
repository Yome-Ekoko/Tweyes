// ** React Imports
import { Fragment, ReactNode, SyntheticEvent, useState } from 'react'

// ** MUI Imports
import Alert, { AlertColor } from '@mui/material/Alert'
import Button from '@mui/material/Button'
import Snackbar from '@mui/material/Snackbar'

// ** Hook Import
import { useSettings } from 'src/@core/hooks/useSettings'
import { AlertTitle } from '@mui/material'

interface CustomNotificationProps {
  severity: AlertColor | undefined
  title: string
  message: ReactNode
}

const CustomNotification = (props: CustomNotificationProps) => {
  const { severity, title, message } = props

  // ** State
  const [open, setOpen] = useState<boolean>(false)

  // ** Hook & Var
  const { settings } = useSettings()
  const { skin } = settings

  const handleClose = (event?: Event | SyntheticEvent, reason?: string) => {
    if (reason === 'clickaway') {
      return
    }
    setOpen(false)
  }

  return (
    <Fragment>
      <Snackbar open={open} onClose={handleClose} autoHideDuration={5000}>
        <Alert
          variant='filled'
          severity={severity}
          onClose={handleClose}
          sx={{ width: '100%' }}
          elevation={skin === 'bordered' ? 0 : 3}
        >
          <AlertTitle>{title}</AlertTitle>
          {message}
        </Alert>
      </Snackbar>
    </Fragment>
  )
}

export default CustomNotification
