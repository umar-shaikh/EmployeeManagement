import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  http = inject(HttpClient);
  router = inject(Router);


  loginObj : any = {
    email : '',
    contactNo: ''
  }

  login(){
    
    this.http.post("https://localhost:7012/api/EmployeeMaster/login", this.loginObj).subscribe({
      next: (res: any) => {
        
         localStorage.setItem("token", JSON.stringify(res.data));
        
        this.router.navigateByUrl("dashboard");
       

      },
      error: (err:any) => {
       
        alert(err.error.message);
      }
    })
  }



}
