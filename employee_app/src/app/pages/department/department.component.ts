import { Component, inject, OnInit } from '@angular/core';
import { DepartmentModel } from '../../models/Department.model';
import { FormsModule } from '@angular/forms';
import { MasterService } from '../../services/master.service';

@Component({
  selector: 'app-department',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './department.component.html',
  styleUrl: './department.component.css'
})
export class DepartmentComponent implements OnInit {

  newDeptObj: DepartmentModel = new DepartmentModel();
  masterService = inject(MasterService);
  deptList: DepartmentModel[] = [];

  ngOnInit(): void {
    this.getAllDepartments();
  }

  saveDepartment(){    
    this.masterService.saveDept(this.newDeptObj).subscribe({
      next: (res: any) => {
        alert("Department saved successfully");
      },
      error: (err:any) => {
        alert(err.error.message);
      }
    });
  
  }

  getAllDepartments(){
    this.masterService.getAllDept().subscribe({
      next: (res: any) => {
        this.deptList = res;
        
      }
    })

  }

  onEdit(item: DepartmentModel){
    const strDate = JSON.stringify(item);
    const obj = JSON.parse(strDate);
    this.newDeptObj = obj;

  }

  resetDept(){
    this.newDeptObj = new DepartmentModel();
  }

   updateDepartment(){    
    this.masterService.updateDept(this.newDeptObj).subscribe({
      next: (res: any) => {
        alert("Department updated successfully");
      },
      error: (err:any) => {
        alert(err.error.message);
      }
    });
  
  }

  onDelete(id: number){
    const confirmDelete = confirm("Are you sure you want to delete this department?");
    if (confirmDelete) {
      this.masterService.deleteDept(id).subscribe({
      next: (res: any) => {
        alert("Department deleted successfully");
      },
      error: (err:any) => {
        alert(err.error.message);
      }
    });
    }
     

  }
}
