// ** MUI Imports
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import DialogTitle from '@mui/material/DialogTitle'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'
import DialogContentText from '@mui/material/DialogContentText'
import { apiDeleteUser, apiResetLockout } from 'src/services/UserService'
import { DeleteUserRequest, ResetLockoutRequest } from 'src/types/requestTypes'

interface ResetLockoutDialogProps {
  open: boolean
  toggle: () => void
  selectedUserId: string
  mutate: () => void
}

const ResetLockoutDialog = (props: ResetLockoutDialogProps) => {
  // ** Props
  const { open, toggle, selectedUserId, mutate } = props

  const handleSubmit = async () => {
    const resetLockoutRequest: ResetLockoutRequest = {
      userName: selectedUserId
    }

    // ** API call to delete user
    await apiResetLockout(resetLockoutRequest)
    mutate()
    toggle()
  }

  return (
    <Dialog
      open={open}
      onClose={toggle}
      aria-labelledby='alert-dialog-title'
      aria-describedby='alert-dialog-description'
    >
      <DialogTitle id='alert-dialog-title'>Are you sure you want to reset this user's lockout?</DialogTitle>
      <DialogContent>
        <DialogContentText id='alert-dialog-description'>This action cannot be undone.</DialogContentText>
      </DialogContent>
      <DialogActions className='dialog-actions-dense'>
        <Button type='submit' variant='tonal' sx={{ mr: 1 }} onClick={toggle}>
          Discard
        </Button>
        <Button variant='contained' color='error' onClick={handleSubmit}>
          Continue
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default ResetLockoutDialog
