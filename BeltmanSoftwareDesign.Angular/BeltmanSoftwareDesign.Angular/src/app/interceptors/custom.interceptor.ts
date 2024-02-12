import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { StateService } from "../services/state.service";
import { Observable } from "rxjs";

@Injectable()
export class CustomInterceptor implements HttpInterceptor {

  constructor(private stateService: StateService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const localToken = this.stateService.getState()?.bearerId;
    if (localToken != null)
      request = request.clone({ headers: request.headers.set('Authorization', 'bearer ' + localToken) });
    return next.handle(request);
  }
}
