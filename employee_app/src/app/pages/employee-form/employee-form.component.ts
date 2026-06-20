import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { RouterModule } from '@angular/router';

import { EmployeeModel } from '../../models/Employee.model';
import { DepartmentModel } from '../../models/Department.model';
import { DesignationModel } from '../../models/Designation.model';

import { EmployeeService } from '../../services/employee.service';
import { MasterService } from '../../services/master.service';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './employee-form.component.html',
  styleUrl: './employee-form.component.css'
})
export class EmployeeFormComponent implements OnInit {

  employeeService = inject(EmployeeService);
  masterService = inject(MasterService);

  employeeList: EmployeeModel[] = [];

  deptList: DepartmentModel[] = [];
  designationList: DesignationModel[] = [];

  newEmployeeObj: EmployeeModel = new EmployeeModel();

  ngOnInit(): void {

    this.getAllEmployees();

    this.getAllDepartments();

    this.getAllDesignations();

  }

  // =========================
  // GET ALL EMPLOYEES
  // =========================

  getAllEmployees() {

    this.employeeService.getAllEmployees().subscribe({

      next: (res: any) => {

        this.employeeList = res;

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // GET ALL DEPARTMENTS
  // =========================

  getAllDepartments() {

    this.masterService.getAllDept().subscribe({

      next: (res: any) => {

        this.deptList = res;

      }

    });

  }

  // =========================
  // GET ALL DESIGNATIONS
  // =========================

  getAllDesignations() {

    this.masterService.getAllDesignation().subscribe({

      next: (res: any) => {

        this.designationList = res;

      }

    });

  }

  // =========================
  // SAVE EMPLOYEE
  // =========================

  saveEmployee() {

    this.employeeService.saveEmployee(this.newEmployeeObj).subscribe({

      next: (res: any) => {

        alert('Employee added successfully');

        this.getAllEmployees();

        this.resetEmployee();

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // EDIT
  // =========================

  onEdit(item: EmployeeModel) {

    const strObj = JSON.stringify(item);

    const parsedObj = JSON.parse(strObj);

    this.newEmployeeObj = parsedObj;

  }

  // =========================
  // UPDATE
  // =========================

  updateEmployee() {

    this.employeeService.updateEmployee(this.newEmployeeObj).subscribe({

      next: (res: any) => {

        alert('Employee updated successfully');

        this.getAllEmployees();

        this.resetEmployee();

      },

      error: (err: any) => {

        alert(err.error.message);

      }

    });

  }

  // =========================
  // DELETE
  // =========================

  onDelete(id: number) {

    const isDelete = confirm('Are you sure you want to delete this employee?');

    if (isDelete) {

      this.employeeService.deleteEmployee(id).subscribe({

        next: (res: any) => {

          alert('Employee deleted successfully');

          this.getAllEmployees();

        },

        error: (err: any) => {

          alert(err.error.message);

        }

      });

    }

  }

  // =========================
  // RESET
  // =========================

  resetEmployee() {

    this.newEmployeeObj = new EmployeeModel();

  }

  // =========================
  // GET DESIGNATION NAME
  // =========================

  getDesignationName(id: number): string {

    const designation = this.designationList.find(
      x => x.designationId == id
    );

    return designation ? designation.designationName : '';

  }

}


