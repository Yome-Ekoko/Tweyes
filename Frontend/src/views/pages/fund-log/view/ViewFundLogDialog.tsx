// ** React Imports
import { Ref, forwardRef, ReactElement } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Dialog from '@mui/material/Dialog'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import Fade, { FadeProps } from '@mui/material/Fade'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'
import IconButton, { IconButtonProps } from '@mui/material/IconButton'

// ** Custom Component Import
import CustomTextField from 'src/@core/components/mui/text-field'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import { GetFundLogResponse } from 'src/types/responseTypes'

const Transition = forwardRef(function Transition(
  props: FadeProps & { children?: ReactElement<any, any> },
  ref: Ref<unknown>
) {
  return <Fade ref={ref} {...props} />
})

const CustomCloseButton = styled(IconButton)<IconButtonProps>(({ theme }) => ({
  top: 0,
  right: 0,
  color: 'grey.500',
  position: 'absolute',
  boxShadow: theme.shadows[2],
  transform: 'translate(10px, -10px)',
  borderRadius: theme.shape.borderRadius,
  backgroundColor: `${theme.palette.background.paper} !important`,
  transition: 'transform 0.25s ease-in-out, box-shadow 0.25s ease-in-out',
  '&:hover': {
    transform: 'translate(7px, -5px)'
  }
}))

interface ViewFundLogDialogProps {
  open: boolean
  toggle: () => void
  selectedFundLog: GetFundLogResponse
}

const ViewFundLogDialog = (props: ViewFundLogDialogProps) => {
  const { open, toggle, selectedFundLog } = props

  return (
    <Dialog
      fullWidth
      open={open}
      maxWidth='md'
      scroll='body'
      onClose={() => toggle()}
      TransitionComponent={Transition}
      onBackdropClick={() => toggle()}
      sx={{ '& .MuiDialog-paper': { overflow: 'visible' } }}
    >
      <DialogContent
        sx={{
          pb: theme => `${theme.spacing(8)} !important`,
          px: theme => [`${theme.spacing(5)} !important`, `${theme.spacing(15)} !important`],
          pt: theme => [`${theme.spacing(8)} !important`, `${theme.spacing(12.5)} !important`]
        }}
      >
        <CustomCloseButton onClick={() => toggle()}>
          <Icon icon='tabler:x' fontSize='1.25rem' />
        </CustomCloseButton>
        <Box sx={{ mb: 8, textAlign: 'center' }}>
          <Typography variant='h3' sx={{ mb: 3 }}>
            View Funding Log
          </Typography>
        </Box>
        <Grid container spacing={6}>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.id} label='Id' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.debitAccountNumber} label='Debit Account Number' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.userId} label='User Id' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.status} label='Status' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.amount} label='Amount' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.attempts} label='Attempts' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.transactionReference} label='Reference' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.responseMessage} label='External Response' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.rvslResponseMessage} label='RVSL Response' disabled />
          </Grid>
          <Grid item sm={6} xs={12}>
            <CustomTextField fullWidth defaultValue={selectedFundLog?.createdDate} label='Date' disabled />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions
        sx={{
          justifyContent: 'center',
          px: theme => [`${theme.spacing(5)} !important`, `${theme.spacing(15)} !important`],
          pb: theme => [`${theme.spacing(8)} !important`, `${theme.spacing(12.5)} !important`]
        }}
      >
        <Button variant='contained' sx={{ mr: 1 }} onClick={() => toggle()}>
          Close
        </Button>
      </DialogActions>
    </Dialog>
  )
}

export default ViewFundLogDialog
