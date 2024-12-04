// ** React Imports
import { useState, useCallback, useEffect } from 'react'

// ** MUI Imports
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import Divider from '@mui/material/Divider'
import MenuItem from '@mui/material/MenuItem'
import Typography from '@mui/material/Typography'
import CardHeader from '@mui/material/CardHeader'
import CardContent from '@mui/material/CardContent'
import { DataGrid, GridColDef } from '@mui/x-data-grid'
import { Button, CircularProgress, LinearProgress, useTheme } from '@mui/material'

// ** Third Party Imports
import DatePicker, { ReactDatePickerProps } from 'react-datepicker'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Custom Components Imports
import ViewFundLogsDialog from 'src/views/pages/fund-log/view/ViewFundLogDialog'
import DatePickerWrapper from 'src/@core/styles/libs/react-datepicker'

// ** Actions Imports
import { useFetchFundLog } from 'src/hooks/useFundLog'

// ** Types Imports
import { GetFundLogResponse } from 'src/types/responseTypes'

// ** Custom Table Components Imports
import TableHeader from 'src/views/pages/fund-log/list/TableHeader'
import { DateType } from 'src/types/generalTypes'
import PickersComponent from 'src/views/components/PickersCustomInput'
import dayjs from 'dayjs'

interface CellType {
  row: GetFundLogResponse
}

interface RowOptionsProps {
  fundlog: GetFundLogResponse
  setSelectedFundLog: (val: GetFundLogResponse) => void
  setViewFundLogOpen: (val: boolean) => void
}

const RowOptions = (props: RowOptionsProps) => {
  const { fundlog, setSelectedFundLog, setViewFundLogOpen } = props

  const handleView = () => {
    setSelectedFundLog(fundlog)
    setViewFundLogOpen(true)
  }

  return (
    <MenuItem onClick={handleView}>
      <Icon icon='tabler:eye' fontSize={20} />
    </MenuItem>
  )
}

const customLinearProgress = () => {
  return (
    <LinearProgress
      sx={{
        height: '1px'
      }}
    />
  )
}

const defaultColumns: GridColDef[] = [
  {
    flex: 0.1,
    field: 'id',
    headerName: 'Id',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
          {row.id}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'debitAccountNumber',
    minWidth: 170,
    headerName: 'Debit Account Number',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
          {row.debitAccountNumber}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'userId',
    minWidth: 170,
    headerName: 'User Id',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
          {row.userId}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'amount',
    minWidth: 170,
    headerName: 'Amount',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
          {row.amount}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'status',
    minWidth: 170,
    headerName: 'Status',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
          {row.status}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 190,
    field: 'createdDate',
    headerName: 'Date Created',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary' }}>
          {dayjs(row.createdDate).format('DD MMM, YYYY')}
        </Typography>
      )
    }
  }
]

const FundLogList = () => {
  const date = new Date()
  // ** State
  const [startDate, setStartDate] = useState<DateType>(undefined)
  const [endDate, setEndDate] = useState<DateType>(new Date())
  const [localStartDate, setLocalStartDate] = useState<DateType>(new Date(date.getFullYear(), 0, 1))
  const [localEndDate, setLocalEndDate] = useState<DateType>(new Date())
  const [value, setValue] = useState<string>('')
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false)
  const [selectedFundLog, setSelectedFundLog] = useState<GetFundLogResponse>({} as GetFundLogResponse)
  const [viewFundLogOpen, setViewFundLogOpen] = useState<boolean>(false)
  const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 10 })

  // ** Hooks
  const { data, pageMeta, isLoading } = useFetchFundLog({
    startDate: startDate,
    endDate: endDate,
    pageNumber: paginationModel.page + 1,
    pageSize: paginationModel.pageSize
  })
  const theme = useTheme()
  const { direction } = theme

  const popperPlacement: ReactDatePickerProps['popperPlacement'] = direction === 'ltr' ? 'bottom-start' : 'bottom-end'

  const handleFilter = useCallback((val: string) => {
    setValue(val)
  }, [])

  const handleSubmit = () => {
    setIsSubmitting(true)
    setStartDate(localStartDate)
    setEndDate(localEndDate)
  }

  useEffect(() => {
    if (!isLoading) {
      setIsSubmitting(false)
    }
  }, [isLoading, isSubmitting])

  const toggleViewFundLogDialog = () => setViewFundLogOpen(!viewFundLogOpen)

  const columns: GridColDef[] = [
    ...defaultColumns,
    {
      flex: 0.1,
      minWidth: 100,
      sortable: false,
      hideable: false,
      filterable: false,
      field: 'actions',
      headerName: 'Actions',
      renderCell: ({ row }: CellType) => (
        <RowOptions fundlog={row} setSelectedFundLog={setSelectedFundLog} setViewFundLogOpen={setViewFundLogOpen} />
      )
    }
  ]

  return (
    <Grid container spacing={6.5}>
      <Grid item xs={12}>
        <Card>
          <CardHeader title='Search Filters' />
          <CardContent>
            <DatePickerWrapper>
              <Grid container spacing={1}>
                <Grid item sm={3} xs={12}>
                  <DatePicker
                    selected={localStartDate}
                    id='start-date-input'
                    popperPlacement={popperPlacement}
                    onChange={(date: Date) => setLocalStartDate(date)}
                    placeholderText='Click to select a start date'
                    customInput={<PickersComponent label='Start Date' />}
                  />
                </Grid>
                <Grid item sm={3} xs={12}>
                  <DatePicker
                    selected={localEndDate}
                    id='end-date-input'
                    popperPlacement={popperPlacement}
                    onChange={(date: Date) => setLocalEndDate(date)}
                    placeholderText='Click to select an end date'
                    customInput={<PickersComponent label='End Date' />}
                  />
                </Grid>
                <Grid item sm={3} xs={12}>
                  <Button
                    variant='contained'
                    sx={{ mr: 1, mt: 5, width: '50%' }}
                    startIcon={isSubmitting && <CircularProgress color='secondary' size={17} />}
                    onClick={handleSubmit}
                    disabled={isSubmitting}
                  >
                    Submit
                  </Button>
                </Grid>
              </Grid>
            </DatePickerWrapper>
          </CardContent>
          <Divider sx={{ m: '0 !important' }} />
          <TableHeader value={value} handleFilter={handleFilter} startDate={startDate} endDate={endDate} />
          <DataGrid
            autoHeight
            pagination
            rowHeight={62}
            rows={data}
            loading={isLoading}
            slots={{
              loadingOverlay: customLinearProgress
            }}
            rowCount={pageMeta?.totalRecords ?? 0}
            columns={columns}
            paginationMode='server'
            disableRowSelectionOnClick
            pageSizeOptions={[10, 25, 50]}
            paginationModel={paginationModel}
            onPaginationModelChange={setPaginationModel}
          />
        </Card>
      </Grid>

      <ViewFundLogsDialog open={viewFundLogOpen} toggle={toggleViewFundLogDialog} selectedFundLog={selectedFundLog} />
    </Grid>
  )
}

FundLogList.acl = {
  action: 'read',
  subject: 'fund-log'
}

export default FundLogList
