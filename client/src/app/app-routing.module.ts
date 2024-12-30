import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginSignupComponent } from './components/login-signup/login-signup.component';
import { HomeComponent } from './components/home/home.component';
import { BookComponent } from './components/book/book.component';

const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'login',component:LoginSignupComponent},
  { path: 'book/:id', component: BookComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
