import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CompaniesComponent } from './components/companies/companies.component';
import { WorkordersComponent } from './components/workorders/workorders.component';
import { UserGaurd } from './guards/user.guard';
import { CompanyGaurd } from './guards/company.guard';
import { NotLoggedInComponent } from './components/notloggedin/notloggedin.component';
import { NoCompanyComponent } from './components/nocompany/nocompany.component';
import { LoginSuccesComponent } from './components/loginsucces/loginsucces.component';
import { RegisterSuccesComponent } from './components/registersucces/registersucces.component';
import { LogoutComponent } from './components/logout/logout.component';
import { LogoutsuccessComponent } from './components/logoutsuccess/logoutsuccess.component';
import { CreateCompanyComponent } from './components/createcompany/createcompany.component';
import { DeleteCompanyComponent } from './components/deletecompany/deletecompany.component';
import { EditCompanyComponent } from './components/editcompany/editcompany.component';
import { NotLoggedInGaurd } from './guards/notloggedin.guard';
import { EditUserComponent } from './components/edituser/edituser.component';
import { PortalSettingsComponent } from './components/portalsettings/portalsettings.component';
import { PortalAccountComponent } from './components/portalaccount/portalaccount.component';
import { PortalCompanyComponent } from './components/portalcompany/portalcompany.component';
import { PortalAnalyticsComponent } from './components/portalanalytics/portalanalytics.component';
import { EditWorkorderComponent } from './components/editworkorder/editworkorder.component';
import { CreateWorkorderComponent } from './components/createworkorder/createworkorder.component';
import { DeleteWorkorderComponent } from './components/deleteworkorder/deleteworkorder.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent,                         canActivate: [NotLoggedInGaurd] },
  { path: 'loginsucces', component: LoginSuccesComponent,             canActivate: [UserGaurd] },
  { path: 'register', component: RegisterComponent,                   canActivate: [NotLoggedInGaurd] },
  { path: 'registersucces', component: RegisterSuccesComponent,       canActivate: [UserGaurd] },
  { path: 'logout', component: LogoutComponent,                       canActivate: [UserGaurd] },
  { path: 'logoutsucces', component: LogoutsuccessComponent,          canActivate: [NotLoggedInGaurd] },
  { path: 'portal-settings', component: PortalSettingsComponent,      canActivate: [CompanyGaurd] },
  { path: 'portal-account', component: PortalAccountComponent,        canActivate: [UserGaurd] },
  { path: 'notloggedin', component: NotLoggedInComponent,             canActivate: [NotLoggedInGaurd] },

  { path: 'nocompany', component: NoCompanyComponent,                 canActivate: [UserGaurd] },
  { path: 'portal-company', component: PortalCompanyComponent,        canActivate: [CompanyGaurd] },
  { path: 'portal-analytics', component: PortalAnalyticsComponent,    canActivate: [CompanyGaurd] },

  { path: 'companies', component: CompaniesComponent,                 canActivate: [UserGaurd] },
  { path: 'createcompany', component: CreateCompanyComponent,         canActivate: [UserGaurd] },
  { path: 'editcompany/:id', component: EditCompanyComponent,         canActivate: [UserGaurd] },
  { path: 'deletecompany/:id', component: DeleteCompanyComponent,     canActivate: [UserGaurd] },

  { path: 'workorders', component: WorkordersComponent, canActivate: [CompanyGaurd] },
  { path: 'createworkorder', component: CreateWorkorderComponent, canActivate: [CompanyGaurd] },
  { path: 'editworkorder/:id', component: EditWorkorderComponent, canActivate: [CompanyGaurd] },
  { path: 'deleteworkorder/:id', component: DeleteWorkorderComponent, canActivate: [CompanyGaurd] },


  { path: 'edituser/:id', component: EditUserComponent,               canActivate: [UserGaurd] },
];
