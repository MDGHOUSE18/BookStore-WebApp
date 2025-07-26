import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { FloatLabelType } from '@angular/material/form-field';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/Services/dataService/data.service';
import { UserService } from 'src/app/Services/userService/user.service';

@Component({
  selector: 'app-login-signup',
  templateUrl: './login-signup.component.html',
  styleUrls: ['./login-signup.component.scss'],
})
export class LoginSignupComponent implements OnInit {
  hide: boolean = true;
  form: boolean = true;
  hideRequiredControl = new FormControl(false);
  floatLabelControl = new FormControl('auto' as FloatLabelType);

  loginForm!: FormGroup;
  signupForm!: FormGroup;

  isPasswordVisible: boolean = false;

  constructor(
    private fb: FormBuilder, 
    private userService: UserService,
    private dialogRef: MatDialogRef<LoginSignupComponent>,
    private dataService:DataService,
    private router:Router,
    private snackBar:MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
          ),
        ],
      ],
    });

    this.signupForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      passwordHash: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
          ),
        ],
      ],
      phone: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  togglePasswordVisibility(): void {
    this.isPasswordVisible = !this.isPasswordVisible;  
  }

  navigateBtn(): void {
    this.form = !this.form; 
  }
  
  onForgotPassport(){
    this.dialogRef.close();
    this.router.navigate(['/forgotPassword']);
  }
  onLoginSubmit(): void {
    if (this.loginForm.valid) {
      this.userService.login(this.loginForm.value).subscribe(
        (response:any) => {
          if (response.success) {
            const { token, name, phone, email } = response.data;
            localStorage.setItem('token', token);
            localStorage.setItem('email', email);
            localStorage.setItem('name', name);
            localStorage.setItem('phone', phone);
            this.dataService.setUsername(name)
            this.dataService.setAccessToken(token)
            this.dataService.setUserEmail(email)
            this.dataService.setUserPhone(phone)
            this.dialogRef.close();
          } else {
            console.error('Login failed: ', response.message);
          }
        },
        (error) => {
          console.error('Login failed:', error);
        }
      );
    } else {
      this.snackBar.open('Credentials are invalid', 'Close', { duration: 2000 });
    }
  }
  
  
  onSignupSubmit(): void {
    if (this.signupForm.valid) {
      this.userService.register(this.signupForm.value).subscribe(
        (response) => {
          this.snackBar.open('Registration successful:'+ response, 'Close', { duration: 2000 });
        },
        (error) => {
          console.error('Registration failed:', error);
        }
      );
    } else {
      this.snackBar.open('ENter all required feilds','Close', { duration: 2000 });
    }
  }

  getFloatLabelValue(): FloatLabelType {
    return this.floatLabelControl.value || 'auto';
  }
}
