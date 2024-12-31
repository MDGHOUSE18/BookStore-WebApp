import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  // encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit{
  userName: string|null = null;
  islogin:boolean =false;
  subsription!:Subscription;
  token:string|null = null;


  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private dataService:DataService
  ) {
    this.checkLoginStatus();
  }
  ngOnInit(): void {
    this.subsription=this.dataService.UserName.subscribe((result)=>this.userName=result)
    this.subsription=this.dataService.AccessToken.subscribe((result)=>this.token=result)
  }

  checkLoginStatus() {
    const storedName = localStorage.getItem('name');
    if (storedName) {
      const fullName = storedName;
      this.userName = fullName.split(' ')[0]; 
      this.islogin = true;
    }
  }


  openLoginDialog() {
    const dialogRef = this.dialog.open(LoginSignupComponent, {
      panelClass: 'transparent-dialog',
    });

    dialogRef.afterClosed();
  }

  onLogout(){
    const confirmLogout = window.confirm('Are you sure you want to log out?');
    if (confirmLogout) {
      localStorage.clear();
      this.dataService.clearUserData()
      console.log(this.token)
      console.log('Logged out and session cleared');
      // alert('logout succesfully')
      this.snackBar.open('Logout successful', 'Close', { duration: 2000 });
      this.islogin=false;
    }
  }
}
