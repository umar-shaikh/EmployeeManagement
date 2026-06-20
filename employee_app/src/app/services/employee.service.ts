import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { EmployeeModel } from '../models/Employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  apiUrl: string = 'https://localhost:7012/api/';
  http = inject(HttpClient);

  // =========================
  // GET ALL EMPLOYEES
  // =========================

  getAllEmployees() {

    return this.http.get(this.apiUrl + 'EmployeeMaster');

  }

  // =========================
  // GET EMPLOYEE BY ID
  // =========================

  getEmployeeById(id: number) {

    return this.http.get(this.apiUrl + 'EmployeeMaster/' + id);

  }

  // =========================
  // ADD EMPLOYEE
  // =========================

  saveEmployee(obj: EmployeeModel) {

    return this.http.post(this.apiUrl + 'EmployeeMaster', obj);

  }

  // =========================
  // UPDATE EMPLOYEE
  // =========================

  updateEmployee(obj: EmployeeModel) {

    return this.http.put(this.apiUrl + 'EmployeeMaster/' + obj.employeeId, obj);

  }

  // =========================
  // DELETE EMPLOYEE
  // =========================

  deleteEmployee(id: number) {

    return this.http.delete(this.apiUrl + 'EmployeeMaster/' + id);

  }

  // =========================
  // FILTER EMPLOYEES
  // =========================

  filterEmployees(
    name?: string,
    city?: string,
    state?: string,
    designationId?: number,
    sortBy: string = 'employeeId',
    sortOrder: string = 'asc',
    pageNumber: number = 1,
    pageSize: number = 10
  ) {

    let params = new HttpParams();

    if (name) {
      params = params.set('name', name);
    }

    if (city) {
      params = params.set('city', city);
    }

    if (state) {
      params = params.set('state', state);
    }

    if (designationId) {
      params = params.set('designationId', designationId);
    }

    params = params.set('sortBy', sortBy);
    params = params.set('sortOrder', sortOrder);
    params = params.set('pageNumber', pageNumber);
    params = params.set('pageSize', pageSize);

    return this.http.get(this.apiUrl + 'EmployeeMaster/filter', {
      params
    });

  }

}
