import { ThemeColor } from 'src/@core/layouts/types'

export type DateType = Date | null | undefined

export interface RoleType {
  [key: string]: { icon: string; color: string }
}

export interface StatusType {
  [key: string]: ThemeColor
}

export interface EnumType {
  [key: string]: number
}

export type EnumTypes = {
  [key: number]: string;
};

export type OptionType = {
  value: string;
  label: string;
}
