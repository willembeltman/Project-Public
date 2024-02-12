import { Injectable, OnInit } from '@angular/core';
import { State } from '../interfaces/state';

@Injectable({
  providedIn: 'root'
})
export class StateService implements OnInit {
  private _localStorageReady: boolean = false;

  constructor() {
    this._localStorageReady = true;
  }

  ngOnInit(): void {
  }

  localStorageReady() {
    this._localStorageReady = true;
  }

  getState(): State | null {
    try {
      if (!this._localStorageReady) return null;
      const state = localStorage.getItem('state');
      if (state == null) return null;
      return JSON.parse(state);
    }
    catch (ex) {
      return null;
    }
  }
  setState(value: State | null) {
    if (!this._localStorageReady) return;
    const stateJson = JSON.stringify(value);
    localStorage.setItem('state', stateJson);
  }

  createStandardRequest() {
    return {
      bearerId: this.getState()?.bearerId ?? null,
      currentCompanyId: this.getState()?.currentCompany?.id ?? null,
    };
  }


  //getState(): State | null {
  //  if (!this._localStorageReady) return null;
  //  try {
  //    return JSON.parse(document.cookie);
  //  }
  //  catch (ex) {
  //    return null;
  //  }
  //}
  //setState(value: State | null) {
  //  if (!this._localStorageReady) return;
  //  document.cookie = JSON.stringify(value);
  //}
}
