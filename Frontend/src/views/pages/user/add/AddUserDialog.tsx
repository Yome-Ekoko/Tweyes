// ** React Imports
import { Ref, forwardRef, ReactElement } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Grid from '@mui/material/Grid'
import Dialog from '@mui/material/Dialog'
import Button from '@mui/material/Button'
import MenuItem from '@mui/material/MenuItem'
import Typography from '@mui/material/Typography'
import Fade, { FadeProps } from '@mui/material/Fade'
import DialogContent from '@mui/material/DialogContent'
import DialogActions from '@mui/material/DialogActions'

// ** Third Party Imports
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import { useForm, Controller } from 'react-hook-form'

// ** Custom Component Import
import CustomTextField from 'src/@core/components/mui/text-field'
import CustomCloseButton from 'src/views/components/CustomCloseButton'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import { AddUserRequest } from 'src/types/requestTypes'
import { apiAddUser } from 'src/services/UserService'

const Transition = forwardRef(function Transition(
  props: FadeProps & { children?: ReactElement<any, any> },
  ref: Ref<unknown>
) {
  return <Fade ref={ref} {...props} />
})

const showErrors = (field: string, valueLen: number, min: number) => {
  if (valueLen === 0) {
    return `${field} field is required`
  } else if (valueLen > 0 && valueLen < min) {
    return `${field} must be at least ${min} characters`
  } else {
    return ''
  }
}

const schema = yup.object().shape({
  userName: yup
    .string()
    .min(3, obj => showErrors('Username', obj.value.length, obj.min))
    .required(),
  name: yup.string().required(),
  email: yup.string().email().required(),
  role: yup.string().required(),
})

const defaultValues = {
  userName: '',
  name: '',
  email: '',
  role: ''
}

const AddUserDialog = (props: any) => {
  const { open, toggle, mutate } = props

  // ** Hooks
  const {
    reset,
    control,
    handleSubmit,
    formState: { errors }
  } = useForm({
    defaultValues,
    mode: 'onChange',
    resolver: yupResolver(schema)
  })
  const onSubmit = async (data: AddUserRequest) => {
    const isSuccess = await apiAddUser(data)
    if (isSuccess) {
      mutate()
      toggle()
      reset()
    }
  }

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
      <form onSubmit={handleSubmit(onSubmit)}>
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
              Add Application User
            </Typography>
          </Box>
          <Grid container spacing={6}>
            <Grid item sm={6} xs={12}>
              <Controller
                name='name'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <CustomTextField
                    fullWidth
                    value={value}
                    sx={{ mb: 4 }}
                    label='Name'
                    onChange={onChange}
                    placeholder='Name of the user'
                    error={Boolean(errors.name)}
                    {...(errors.name && { helperText: errors.name.message })}
                  />
                )}
              />
            </Grid>
            <Grid item sm={6} xs={12}>
              <Controller
                name='userName'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <CustomTextField
                    fullWidth
                    value={value}
                    sx={{ mb: 4 }}
                    label='User Name'
                    onChange={onChange}
                    placeholder='UserName of the user'
                    error={Boolean(errors.userName)}
                    {...(errors.userName && { helperText: errors.userName.message })}
                  />
                )}
              />
            </Grid>
            {/** [TODO] Make the list of option fetch from the server side */}
            <Grid item sm={6} xs={12}>
              <Controller
                name='role'
                control={control}
                // rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <CustomTextField
                    select
                    fullWidth
                    sx={{ mb: 4 }}
                    label='User Role'
                    id='validation-role-select'
                    error={Boolean(errors.role)}
                    aria-describedby='validation-role-select'
                    {...(errors.role && { helperText: errors.role.message })}
                    SelectProps={{
                      value: value,
                      displayEmpty: true,
                      onChange: e => onChange(e)
                    }}
                  >
                  <MenuItem value={''}>Select role</MenuItem>
                  <MenuItem value={'Administrator'}>Administrator</MenuItem>
                  <MenuItem value={'Log'}>Log</MenuItem>
                  </CustomTextField>
                )}
              />
            </Grid>
            <Grid item sm={6} xs={12}>
              <Controller
                name='email'
                control={control}
                rules={{ required: true }}
                render={({ field: { value, onChange } }) => (
                  <CustomTextField
                    fullWidth
                    value={value}
                    sx={{ mb: 4 }}
                    label='Email Address'
                    onChange={onChange}
                    placeholder='Email address of the user'
                    error={Boolean(errors.email)}
                    {...(errors.email && { helperText: errors.email.message })}
                  />
                )}
              />
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
          <Button type='submit' variant='contained' sx={{ mr: 1 }}>
            Submit
          </Button>
          <Button variant='tonal' color='secondary' onClick={() => toggle()}>
            Discard
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}

export default AddUserDialog
