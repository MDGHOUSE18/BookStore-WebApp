import { Component, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginSignupComponent } from '../login-signup/login-signup.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  // encapsulation: ViewEncapsulation.None
})
export class HeaderComponent {
  userName: string = 'Profile';
  islogin:boolean =false;

  constructor(private dialog: MatDialog,private snackBar: MatSnackBar) {
    this.checkLoginStatus();
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

    dialogRef.afterClosed().subscribe((result) => {
      if (result && result.success) {
        const fullName = result.data.name; 
        localStorage.setItem('name', fullName);
        this.userName = fullName.split(' ')[0]; 
        this.islogin = true;
        this.snackBar.open('Login successful', 'Close', { duration: 2000 });
      }
    });
  }

  onLogout(){
    const confirmLogout = window.confirm('Are you sure you want to log out?');
    if (confirmLogout) {
      localStorage.clear();
      console.log('Logged out and session cleared');
      // alert('logout succesfully')
      this.snackBar.open('Logout successful', 'Close', { duration: 2000 });
      this.islogin=false;
    }
  }
}
