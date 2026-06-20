import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DepartmentComponent } from '../pages/department/department.component';
import { DepartmentModel } from '../models/Department.model';
import { DesignationModel } from '../models/Designation.model';

@Injectable({
  providedIn: 'root'
})
export class MasterService {
  apiUrl: string = 'https://localhost:7012/api/';
  http = inject(HttpClient);

  getAllDept(){
    return this.http.get(this.apiUrl + 'DepartmentMaster/GetAllDepartments');
  }

  saveDept(Obj: DepartmentModel){
    return this.http.post(this.apiUrl + 'DepartmentMaster/AddDepartment', Obj);
  }

  updateDept(Obj: DepartmentModel){
    return this.http.put(this.apiUrl + 'DepartmentMaster/UpdateDepartment', Obj);
  }

  deleteDept(id: number){
    return this.http.delete(this.apiUrl + 'DepartmentMaster/DeleteDepartment/' + id);
  }


  // =========================
  // Designation APIs
  // =========================

  getAllDesignation() {
    return this.http.get(this.apiUrl + 'DesignationMaster');
  }

  getDesignationById(id: number) {
    return this.http.get(this.apiUrl + 'DesignationMaster/' + id);
  }

  saveDesignation(obj: DesignationModel) {
    return this.http.post(this.apiUrl + 'DesignationMaster', obj);
  }

  updateDesignation(obj: DesignationModel) {
    return this.http.put(this.apiUrl + 'DesignationMaster/' + obj.designationId, obj);
  }

  deleteDesignation(id: number) {
    return this.http.delete(this.apiUrl + 'DesignationMaster/' + id);
  }
}
