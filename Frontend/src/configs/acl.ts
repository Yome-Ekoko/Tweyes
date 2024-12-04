import { AbilityBuilder, Ability } from '@casl/ability'

export type Subjects = string
export type Actions = 'manage' | 'create' | 'read' | 'update' | 'delete'

export type AppAbility = Ability<[Actions, Subjects]> | undefined

export const AppAbility = Ability as any
export type ACLObj = {
  action: Actions
  subject: string
}

const defineRulesFor = (roles: string[], subject: string) => {
  const { can, rules } = new AbilityBuilder(AppAbility)

  roles.map((role) => {
    switch (role) {
      case 'Administrator':
        can('manage', 'user-management')
        can('manage', 'account-log')
        can('manage', 'fund-log')
        can('manage', 'withdraw-log')
        break;
      case 'Log':
        can('manage', 'account-log')
        can('manage', 'fund-log')
        can('manage', 'withdraw-log')
        break;
      default:
        break;
    }
  })

  return rules
}

export const buildAbilityFor = (roles: string[], subject: string): AppAbility => {
  return new AppAbility(defineRulesFor(roles, subject), {
    // https://casl.js.org/v5/en/guide/subject-type-detection
    // @ts-ignore
    detectSubjectType: object => object!.type
  })
}

export const defaultACLObj: ACLObj = {
  action: 'manage',
  subject: 'all'
}

export default defineRulesFor
