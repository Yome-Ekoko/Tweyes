import { saveAs } from 'file-saver'

// ** MUI Imports
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'

// ** Custom Component Import
import CustomTextField from 'src/@core/components/mui/text-field'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import { DateType } from 'src/types/generalTypes'
import { apiDownloadAccountLogs } from 'src/services/AccountLogService'
import { CircularProgress } from '@mui/material'
import { useState } from 'react'

interface TableHeaderProps {
  value: string
  handleFilter: (val: string) => void
  startDate: DateType
  endDate: DateType
}

const TableHeader = (props: TableHeaderProps) => {
  // ** Props
  const { handleFilter, value, startDate, endDate } = props

  const [isSubmitting, setIsSubmitting] = useState<boolean>(false)

  const handleDownload = async () => {
    setIsSubmitting(true)
    const response = await apiDownloadAccountLogs({ startDate, endDate })
    setIsSubmitting(false)
    if (response?.data) {
      const filename = `accountlog-report-${new Date().toISOString()}.csv`
      const url = URL.createObjectURL(new Blob([response.data]))
      saveAs(url, filename)
    }
  }

  return (
    <Box
      sx={{
        py: 4,
        px: 6,
        rowGap: 2,
        columnGap: 4,
        display: 'flex',
        flexWrap: 'wrap',
        alignItems: 'center',
        justifyContent: 'space-between'
      }}
    >
      <Button
        color='primary'
        variant='tonal'
        onClick={handleDownload}
        startIcon={isSubmitting ? <CircularProgress color='secondary' size={17} /> : <Icon icon='tabler:upload' />}
        disabled={isSubmitting}
      >
        Export
      </Button>
    </Box>
  )
}

export default TableHeader
