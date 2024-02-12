//import { Injectable } from '@angular/core';
//import { StateService } from './state.service';
//import { Observable, of, tap } from 'rxjs';
//import { CountriesService } from '../apiservices/countries.service';
//import { CountryListResponse } from '../interfaces/response/countrylistresponse';
//import { CompanyListResponse } from '../interfaces/response/companylistresponse';
//import { CompaniesService } from '../apiservices/companies.service';

//@Injectable({
//  providedIn: 'root'
//})
//export class CacheService {
//  constructor(
//    private stateService: StateService,
//    private countriesService: CountriesService,
//    private companiesService: CompaniesService
//  ) {
//  }

//  private _countries: CountryListResponse | null = null;
//  getCountries(): Observable<CountryListResponse> {
//    if (this._countries == null) {
//      return this.countriesService
//        .list({
//          bearerId: this.stateService.getState()?.bearerId ?? null,
//          currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null
//        })
//        .pipe(
//          tap(response => {
//            this.stateService.setState(response.state);
//            if (response.success) {
//              this._countries = response;
//            }
//          })
//        );
//    } else {
//      return of(this._countries); // Retourneer de opgeslagen data
//    }
//  }
//  resetCountries() {
//    this._countries = null;
//  }

//  private _companies: CompanyListResponse | null = null;
//  getCompanies(): Observable<CompanyListResponse> {
//    if (this._companies == null) {
//      return this.companiesService
//        .list({
//          bearerId: this.stateService.getState()?.bearerId ?? null,
//          currentCompanyId: this.stateService.getState()?.currentCompany?.id ?? null
//        })
//        .pipe(
//          tap(response => {
//            this.stateService.setState(response.state);
//            if (response.success) {
//              this._companies = response;
//            }
//          })
//        );
//    } else {
//      return of(this._companies); // Retourneer de opgeslagen data
//    }
//  }
//  resetCompanies() {
//    this._companies = null;
//  }

//}
