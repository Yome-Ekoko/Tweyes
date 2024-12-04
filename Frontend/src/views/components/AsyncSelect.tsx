// ** React Imports
import { Fragment, useState } from 'react'

// ** MUI Import
import CircularProgress from '@mui/material/CircularProgress'

// ** Custom Component Imports
import CustomTextField from 'src/@core/components/mui/text-field'
import CustomAutocomplete from 'src/@core/components/mui/autocomplete'
import { OptionType } from 'src/types/generalTypes'
import { SelectChangeEvent } from '@mui/material'

interface AsyncSelectProps {
  apiLoading: boolean,
  options: OptionType[],
  label: string,
  handleChange: (value: OptionType | null) => void
}

const AsyncSelect = (props: AsyncSelectProps) => {
  // ** Props
  const { apiLoading, options, label, handleChange } = props
  // ** States
  const [open, setOpen] = useState<boolean>(false)

  const loading = open && apiLoading

  return (
    <CustomAutocomplete
      open={open}
      options={options}
      loading={loading}
      onOpen={() => setOpen(true)}
      onClose={() => setOpen(false)}
      onChange={(e, value) => handleChange(value)}
      id='autocomplete-asynchronous-request'
      getOptionLabel={option => option.label || ''}
      isOptionEqualToValue={(option, value) => option.value === value.value}
      renderInput={params => (
        <CustomTextField
          {...params}
          label={label}
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <Fragment>
                {loading ? <CircularProgress size={20} /> : null}
                {params.InputProps.endAdornment}
              </Fragment>
            )
          }}
        />
      )}
    />
  )
}

export default AsyncSelect
