import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main.component';

export const mainRoutes: Routes = [
  { path: '', redirectTo: 'trang-chu', pathMatch: 'full' },
  {
    path: '',
    component: MainComponent,
    children: [
      {
        path: 'trang-chu',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'quan-ly-tai-khoan',
        loadChildren: () =>
          import('./user/user.module').then((m) => m.UserModule),
      },
      {
        path: 'phan-quyen-tai-khoan',
        loadChildren: () =>
          import('./user-group/user-group.module').then(
            (m) => m.UserGroupModule
          ),
      },
      {
        path: 'quan-ly-chuc-nang',
        loadChildren: () =>
          import('./object-menu/object-menu.module').then(
            (m) => m.ObjectMenuModule
          ),
      },
      {
        path: 'quan-ly-dang-nhap',
        loadChildren: () =>
          import('./location-tracking/location-tracking.module').then(
            (m) => m.LocationTrackingModule
          ),
      },
      {
        path: 'quan-ly-thay-doi-du-lieu',
        loadChildren: () =>
          import('./audit-change/audit-change.module').then(
            (m) => m.AuditChangeModule
          ),
      },
      {
        path: 'quan-ly-xoa-du-lieu-cu',
        loadChildren: () =>
          import('./delete-data/delete-data.module').then(
            (m) => m.DeleteDataModule
          ),
      },
      {
        path: 'chuc-nang-demo',
        loadChildren: () =>
          import('./test-demo/test-demo.module').then((m) => m.TestDemoModule),
      },
      {
        path: 'quan-ly-vung',
        loadChildren: () =>
          import('./administrative-region/administrative-region.module').then(
            (m) => m.AdministrativeRegionModule
          ),
      },
      {
        path: 'quan-ly-don-vi',
        loadChildren: () =>
          import('./administrative-unit/administrative-unit.module').then(
            (m) => m.AdministrativeUnitModule
          ),
      },
      {
        path: 'quan-ly-tinh-thanh',
        loadChildren: () =>
          import(
            './administrative-province/administrative-province.module'
          ).then((m) => m.AdministrativeProvinceModule),
      },
      {
        path: 'quan-ly-quan-huyen',
        loadChildren: () =>
          import(
            './administrative-district/administrative-district.module'
          ).then((m) => m.AdministrativeDistrictModule),
      },
      {
        path: 'quan-ly-phuong-xa',
        loadChildren: () =>
          import('./administrative-ward/administrative-ward.module').then(
            (m) => m.AdministrativeWardModule
          ),
      },
      {
        path: 'nguoi-dung',
        loadChildren: () =>
          import('./person/person.module').then((m) => m.PersonModule),
      },
      {
        path: 'kham-benh',
        loadChildren: () =>
          import('./medical-examination/medical-examination.module').then(
            (m) => m.MedicalExaminationModule
          ),
      },
      {
        path: 'kho-thuoc-le',
        loadChildren: () =>
          import('./medical-warehouse-odd/medical-warehouse-odd.module').then(
            (m) => m.MedicalWarehouseOddModule
          ),
      },
      {
        path: 'kho-thuoc-chinh',
        loadChildren: () =>
          import('./medical-warehouse-main/medical-warehouse-main.module').then(
            (m) => m.MedicalWarehouseMainModule
          ),
      },
      {
        path: 'nha-cung-cap',
        loadChildren: () =>
          import('./supplier/supplier.module').then((m) => m.SupplierModule),
      },
      {
        path: 'don-vi-tinh',
        loadChildren: () =>
          import('./unit-measure/unit-measure.module').then(
            (m) => m.UnitMeasureModule
          ),
      },
      {
        path: 'thuoc-sap-het-han',
        loadChildren: () =>
          import(
            './medical-warehouse-expiry/medical-warehouse-expiry.module'
          ).then((m) => m.MedicalWarehouseExpiryModule),
      },
      {
        path: 'benh-nhan',
        loadChildren: () =>
          import('./patient/patient.module').then((m) => m.PatientModule),
      },
      {
        path: 'nhap-ket-qua',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'danh-muc-khach-hang',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'khach-hang-360',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'ql-vung',
        loadChildren: () =>
          import('./orgchart/region/region.module').then(
            (m) => m.RegionModule
          ),
      },
      {
        path: 'ql-khu-vuc',
        loadChildren: () =>
          import('./orgchart/area/area.module').then(
            (m) => m.AreaModule
          ),
      },
      {
        path: 'ql-chi-nhanh',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'ql-phong-ban',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'ql-san-pham',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'ql-nhu-cau-khach-hang',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
      {
        path: 'ql-co-hoi-ban-hang',
        loadChildren: () =>
          import('./patient-result/patient-result.module').then(
            (m) => m.PatientResultModule
          ),
      },
    ],
  },
];
@NgModule({
  imports: [RouterModule.forChild(mainRoutes)],
  exports: [RouterModule],
})
export class MainRoutingModule {}
