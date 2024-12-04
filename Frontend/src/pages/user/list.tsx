// ** React Imports
import { useState, MouseEvent, useCallback } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Menu from '@mui/material/Menu'
import Grid from '@mui/material/Grid'
import Divider from '@mui/material/Divider'
import MenuItem from '@mui/material/MenuItem'
import IconButton from '@mui/material/IconButton'
import Typography from '@mui/material/Typography'
import CardHeader from '@mui/material/CardHeader'
import CardContent from '@mui/material/CardContent'
import { DataGrid, GridColDef } from '@mui/x-data-grid'
import { SelectChangeEvent } from '@mui/material/Select'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Custom Components Imports
import CustomChip from 'src/@core/components/mui/chip'
import CustomAvatar from 'src/@core/components/mui/avatar'
import CustomTextField from 'src/@core/components/mui/text-field'

// ** Utils Import
import { getInitials } from 'src/@core/utils/get-initials'

// ** Actions Imports
import { useFetchUser } from 'src/hooks/useUser'

// ** Types Imports
import { ThemeColor } from 'src/@core/layouts/types'

// ** Custom Table Components Imports
import TableHeader from 'src/views/pages/user/list/TableHeader'
import AddUserDialog from 'src/views/pages/user/add/AddUserDialog'
import { GetUserResponse } from 'src/types/responseTypes'
import EditUserDialog from 'src/views/pages/user/edit/EditUserDialog'
import ViewUsersDialog from 'src/views/pages/user/view/ViewUsersDialog'
import { LinearProgress } from '@mui/material'
import DeleteUserDialog from 'src/views/pages/user/delete/DeleteUserDialog'
import ResetLockoutDialog from 'src/views/pages/user/reset-lockout/ResetLockoutDialog'
import dayjs from 'dayjs'

interface UserRoleType {
  [key: string]: { icon: string; color: string }
}

interface UserStatusType {
  [key: string]: ThemeColor
}

interface LockoutStatusType {
  [key: string]: ThemeColor
}

interface CellType {
  row: GetUserResponse
}

// ** renders client column
const userRoleObj: UserRoleType = {
  Administrator: { icon: 'tabler:device-laptop', color: 'secondary' },
  Log: { icon: 'tabler:user', color: 'warning' }
}

const userStatusObj: UserStatusType = {
  1: 'success',
  2: 'warning'
}

const lockoutStatusObj: LockoutStatusType = {
  false: 'success',
  true: 'error'
}

// ** renders client column
const renderClient = (row: GetUserResponse) => {
  return (
    <CustomAvatar
      skin='light'
      color='success'
      sx={{ mr: 2.5, width: 38, height: 38, fontWeight: 500, fontSize: theme => theme.typography.body1.fontSize }}
    >
      {getInitials(row.name)}
    </CustomAvatar>
  )
}

interface RowOptionsProps {
  user: GetUserResponse
  setSelectedUser: (val: GetUserResponse) => void
  setEditUserOpen: (val: boolean) => void
  setViewUserOpen: (val: boolean) => void
  setDeleteUserOpen: (val: boolean) => void
  setResetLockoutOpen: (val: boolean) => void
}

const RowOptions = (props: RowOptionsProps) => {
  const { user, setSelectedUser, setViewUserOpen, setEditUserOpen, setDeleteUserOpen, setResetLockoutOpen } = props
  // ** State
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)

  const rowOptionsOpen = Boolean(anchorEl)

  const handleRowOptionsClick = (event: MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget)
  }
  const handleRowOptionsClose = () => {
    setAnchorEl(null)
  }

  const handleView = () => {
    setSelectedUser(user)
    setViewUserOpen(true)
    handleRowOptionsClose()
  }

  const handleEdit = () => {
    setSelectedUser(user)
    setEditUserOpen(true)
    handleRowOptionsClose()
  }

  const handleDelete = () => {
    setSelectedUser(user)
    setDeleteUserOpen(true)
    handleRowOptionsClose()
  }

  const handleResetLockout = () => {
    setSelectedUser(user)
    setResetLockoutOpen(true)
    handleRowOptionsClose()
  }

  return (
    <>
      <IconButton size='small' onClick={handleRowOptionsClick}>
        <Icon icon='tabler:dots-vertical' />
      </IconButton>
      <Menu
        keepMounted
        anchorEl={anchorEl}
        open={rowOptionsOpen}
        onClose={handleRowOptionsClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right'
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right'
        }}
        PaperProps={{ style: { minWidth: '8rem' } }}
      >
        <MenuItem onClick={handleView} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='tabler:eye' fontSize={20} />
          View
        </MenuItem>
        <MenuItem onClick={handleEdit} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='tabler:edit' fontSize={20} />
          Edit
        </MenuItem>
        <MenuItem onClick={handleDelete} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='tabler:trash' fontSize={20} />
          Delete
        </MenuItem>
        <MenuItem onClick={handleResetLockout} sx={{ '& svg': { mr: 2 } }}>
          <Icon icon='tabler:refresh-dot' fontSize={20} />
          Reset Lockout
        </MenuItem>
      </Menu>
    </>
  )
}

const customLinearProgress = () => {
  return <LinearProgress sx={{
    height: '1px'
  }} />
}

const defaultColumns: GridColDef[] = [
  {
    flex: 0.15,
    minWidth: 190,
    field: 'userName',
    headerName: 'UserName',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary' }}>
          {row.userName}
        </Typography>
      )
    }
  },
  {
    flex: 0.15,
    field: 'defaultRole',
    minWidth: 170,
    headerName: 'Role',
    renderCell: ({ row }: CellType) => {
      return (
        row.roles.map((role, index) => (
          <Box key={index} sx={{ display: 'flex', alignItems: 'center' }}>
            <CustomAvatar
              skin='light'
              sx={{ mr: 4, width: 30, height: 30 }}
              color={(userRoleObj[role]?.color as ThemeColor) || 'primary'}
            >
              <Icon icon={userRoleObj[role]?.icon} />
            </CustomAvatar>
            <Typography noWrap sx={{ color: 'text.secondary', textTransform: 'capitalize' }}>
              {role}
            </Typography>
          </Box>
        ))
      )
    }
  },
  {
    flex: 0.15,
    minWidth: 110,
    field: 'createdAt',
    headerName: 'Date Created',
    renderCell: ({ row }: CellType) => {
      return (
        <Typography noWrap sx={{ color: 'text.secondary' }}>
          {dayjs(row.createdAt).format('DD MMM, YYYY')}
        </Typography>
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 190,
    field: 'lockoutEnd',
    headerName: 'Lockout Status',
    renderCell: ({ row }: CellType) => {
      const isLockedOut = row.lockoutEnabled && row.lockoutEnd && new Date(row.lockoutEnd) > new Date() ? 'true' : 'false'
      return (
        <CustomChip
          rounded
          skin='light'
          size='small'
          label={isLockedOut === 'true' ? 'Locked' : 'Open'}
          color={lockoutStatusObj[isLockedOut]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  },
  {
    flex: 0.1,
    minWidth: 110,
    field: 'status',
    headerName: 'Status',
    renderCell: ({ row }: CellType) => {
      return (
        <CustomChip
          rounded
          skin='light'
          size='small'
          label={row.statusMeaning}
          color={userStatusObj[row.status]}
          sx={{ textTransform: 'capitalize' }}
        />
      )
    }
  }
]

const UserList = () => {
  // ** State
  const [role, setRole] = useState<string>('')
  const [value, setValue] = useState<string>('')
  const [status, setStatus] = useState<number>(0)
  const [selectedUser, setSelectedUser] = useState<GetUserResponse>({} as GetUserResponse)
  const [addUserOpen, setAddUserOpen] = useState<boolean>(false)
  const [editUserOpen, setEditUserOpen] = useState<boolean>(false)
  const [viewUserOpen, setViewUserOpen] = useState<boolean>(false)
  const [deleteUserOpen, setDeleteUserOpen] = useState<boolean>(false)
  const [resetLockoutOpen, setResetLockoutOpen] = useState<boolean>(false)
  const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 10 })

  // ** Hooks
  const { data, pageMeta, isLoading, mutate } = useFetchUser({
    query: value,
    role: role,
    status: status > 0 ? status : undefined,
    pageNumber: paginationModel.page + 1,
    pageSize: paginationModel.pageSize
  })

  const handleFilter = useCallback((val: string) => {
    setValue(val)
  }, [])

  const handleRoleChange = useCallback((e: SelectChangeEvent<unknown>) => {
    setRole(e.target.value as string)
  }, [])

  const handleStatusChange = useCallback((e: SelectChangeEvent<unknown>) => {
    setStatus(e.target.value as number)
  }, [])

  const handleView = (user: GetUserResponse) => {
    setSelectedUser(user)
    setViewUserOpen(true)
  }

  const toggleAddUserDialog = () => setAddUserOpen(!addUserOpen)
  const toggleViewUserDialog = () => setViewUserOpen(!viewUserOpen)
  const toggleEditUserDialog = () => setEditUserOpen(!editUserOpen)
  const toggleDeleteUserDialog = () => setDeleteUserOpen(!deleteUserOpen)
  const toggleResetLockoutDialog = () => setResetLockoutOpen(!resetLockoutOpen)

  const columns: GridColDef[] = [
    {
      flex: 0.25,
      minWidth: 280,
      field: 'firstName',
      headerName: 'User',
      renderCell: ({ row }: CellType) => {
        const { name, email } = row

        return (
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            {renderClient(row)}
            <Box sx={{ display: 'flex', alignItems: 'flex-start', flexDirection: 'column' }}>
              <Typography
                noWrap
                onClick={() => handleView(row)}
                sx={{
                  fontWeight: 500,
                  textDecoration: 'none',
                  color: 'text.secondary',
                  '&:hover': { color: 'primary.main' }
                }}
              >
                {name}
              </Typography>
              <Typography noWrap variant='body2' sx={{ color: 'text.disabled' }}>
                {email}
              </Typography>
            </Box>
          </Box>
        )
      }
    },
    ...defaultColumns,
    {
      flex: 0.1,
      minWidth: 100,
      sortable: false,
      field: 'actions',
      headerName: 'Actions',
      renderCell: ({ row }: CellType) => (
        <RowOptions
          user={row}
          setSelectedUser={setSelectedUser}
          setEditUserOpen={setEditUserOpen}
          setViewUserOpen={setViewUserOpen}
          setDeleteUserOpen={setDeleteUserOpen}
          setResetLockoutOpen={setResetLockoutOpen}
        />
      )
    }
  ]

  return (
    <Grid container spacing={6.5}>
      <Grid item xs={12}>
        <Card>
          <CardHeader title='Search Filters' />
          <CardContent>
            <Grid container spacing={6}>
              <Grid item sm={6} xs={12}>
                <CustomTextField
                  select
                  fullWidth
                  defaultValue='Select Role'
                  SelectProps={{
                    value: role,
                    displayEmpty: true,
                    onChange: e => handleRoleChange(e)
                  }}
                >
                  <MenuItem value={''}>Select Role</MenuItem>
                  <MenuItem value={'Administrator'}>Administrator</MenuItem>
                  <MenuItem value={'Log'}>Log User</MenuItem>
                </CustomTextField>
              </Grid>
              <Grid item sm={6} xs={12}>
                <CustomTextField
                  select
                  fullWidth
                  defaultValue='Select Status'
                  SelectProps={{
                    value: status,
                    displayEmpty: true,
                    onChange: e => handleStatusChange(e)
                  }}
                >
                  <MenuItem value={0}>Select Status</MenuItem>
                  <MenuItem value={1}>Active</MenuItem>
                  <MenuItem value={2}>Inactive</MenuItem>
                </CustomTextField>
              </Grid>
            </Grid>
          </CardContent>
          <Divider sx={{ m: '0 !important' }} />
          <TableHeader value={value} handleFilter={handleFilter} toggle={toggleAddUserDialog} />
          <DataGrid
            autoHeight
            pagination
            rowHeight={62}
            rows={data}
            loading={isLoading}
            slots={{
              loadingOverlay: customLinearProgress,
            }}
            rowCount={pageMeta?.totalRecords ?? 0}
            columns={columns}
            paginationMode='server'
            disableRowSelectionOnClick
            pageSizeOptions={[2, 10, 25, 50]}
            paginationModel={paginationModel}
            onPaginationModelChange={setPaginationModel}
          />
        </Card>
      </Grid>

      <AddUserDialog open={addUserOpen} toggle={toggleAddUserDialog} mutate={mutate} />
      <EditUserDialog open={editUserOpen} toggle={toggleEditUserDialog} existingUser={selectedUser} mutate={mutate} />
      <ViewUsersDialog open={viewUserOpen} toggle={toggleViewUserDialog} existingUser={selectedUser} />
      <DeleteUserDialog open={deleteUserOpen} toggle={toggleDeleteUserDialog} selectedUserId={selectedUser.userName} mutate={mutate} />
      <ResetLockoutDialog open={resetLockoutOpen} toggle={toggleResetLockoutDialog} selectedUserId={selectedUser.userName} mutate={mutate} />

    </Grid>
  )
}

UserList.acl = {
  action: 'read',
  subject: 'user-management'
}

export default UserList
