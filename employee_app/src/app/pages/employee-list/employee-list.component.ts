import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { EmployeeService } from '../../services/employee.service';
import { EmployeeModel } from '../../models/Employee.model';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent implements OnInit {

  activatedRoute = inject(ActivatedRoute);
  employeeService = inject(EmployeeService);

  employeeObj: EmployeeModel = new EmployeeModel();

  ngOnInit(): void {

    const empId = Number(this.activatedRoute.snapshot.paramMap.get('id'));

    this.getEmployeeById(empId);

  }

  getEmployeeById(id: number) {

    this.employeeService.getEmployeeById(id).subscribe({

      next: (res: any) => {

        this.employeeObj = res;

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

}
