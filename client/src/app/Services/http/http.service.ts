import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  constructor(private http : HttpClient) { }

  postService(url:string,reqDate:any,token:boolean=false,httpOptions:any={}){
    return this.http.post(url,reqDate,token && httpOptions)
  }

  getService(url: string): Observable<any>{
    return this.http.get(url);
  }
}
