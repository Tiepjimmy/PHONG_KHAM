import { Routes } from '@angular/router';

// Routes for /admin/account/*
// Each page is lazy loaded: its code is only downloaded when the route is visited.
export const ACCOUNT_ROUTES: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  {
    path: 'overview',
    loadComponent: () => import('./account-overview/account-overview.component').then(m => m.AccountOverviewComponent),
  },
  {
    path: 'settings',
    loadComponent: () => import('./account-settings/account-settings.component').then(m => m.AccountSettingsComponent),
  },
  {
    path: 'security',
    loadComponent: () => import('./account-security/account-security.component').then(m => m.AccountSecurityComponent),
  },
  {
    path: 'activity',
    loadComponent: () => import('./account-activity/account-activity.component').then(m => m.AccountActivityComponent),
  },
  {
    path: 'billing',
    loadComponent: () => import('./account-billing/account-billing.component').then(m => m.AccountBillingComponent),
  },
  {
    path: 'statements',
    loadComponent: () => import('./account-statements/account-statements.component').then(m => m.AccountStatementsComponent),
  },
  {
    path: 'referrals',
    loadComponent: () => import('./account-referrals/account-referrals.component').then(m => m.AccountReferralsComponent),
  },
  {
    path: 'api-keys',
    loadComponent: () => import('./account-api-keys/account-api-keys.component').then(m => m.AccountApiKeysComponent),
  },
  {
    path: 'logs',
    loadComponent: () => import('./account-logs/account-logs.component').then(m => m.AccountLogsComponent),
  },
];
