using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Repositories
{
    /// <summary>
    /// ApplicationInitializer class
    /// </summary>
    /// <remarks>
    /// This class permits to define data tests
    /// </remarks>
    public static class ApplicationInitializer
    {
        /// <summary>
        /// This method permits to initialize the data tests.
        /// </summary>
        /// <param name="context">ApplicationContext</param>
        public static void Initialize(ApplicationContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                /*
                // If users exist in database then there are data rows.
                if (context.Users.Any())
                {
                    return;
                }

                // Dimension
                Dimension[] dimensions = new Dimension[]
                {
                    new Dimension { Name = "Secto", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now },
                    new Dimension { Name = "Produit", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now },
                };

                foreach (Dimension s in dimensions)
                {
                    context.Dimension.Add(s);
                }
                context.SaveChanges();

                // Dimension Group Data
                DimensionGroupData[] dimensionGroupDatas = new DimensionGroupData[]
                {
                    new DimensionGroupData { ValueKey = "groupData1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now },
                    new DimensionGroupData { ValueKey = "groupData2", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now },
                };

                foreach (DimensionGroupData s in dimensionGroupDatas)
                {
                    context.DimensionGroupData.Add(s);
                }
                context.SaveChanges();

                // Dimension Period Data
                DimensionPeriodData[] dimensionPeriods = new DimensionPeriodData[]
                {
                    new DimensionPeriodData { ValueKey ="period1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now }
                };

                foreach (DimensionPeriodData s in dimensionPeriods)
                {
                    context.DimensionPeriodData.Add(s);
                }
                context.SaveChanges();

                // Dimension Scalar Data
                DimensionScalarData[] dimensionScalars = new DimensionScalarData[]
                {
                    new DimensionScalarData { ValueKey ="scalar1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now }
                };

                foreach (DimensionScalarData s in dimensionScalars)
                {
                    context.DimensionScalarData.Add(s);
                }
                context.SaveChanges();

                // Dimension Tree Data
                DimensionTreeData[] dimensionTrees = new DimensionTreeData[]
                {
                    new DimensionTreeData { ValueKey ="tree1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now }
                };

                foreach (DimensionTreeData s in dimensionTrees)
                {
                    context.DimensionTreeData.Add(s);
                }
                context.SaveChanges();

                // Dimension Values Data
                DimensionValues[] dimensionValues = new DimensionValues[]
                {
                    new DimensionValues { ValueKey ="values1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now }
                };

                foreach (DimensionValues s in dimensionValues)
                {
                    context.DimensionValues.Add(s);
                }
                context.SaveChanges();

                // DataSet
                DataSet[] datasets = new DataSet[]
                {
                    new DataSet { Name = "DataSet1", Username = "MigrationUser", AddedDate = DateTime.Now,  ModifiedDate = DateTime.Now }
                };

                foreach (DataSet s in datasets)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.DataSet.Add(s);
                    context.SaveChanges();
                }

                DataSet dataSet = datasets[0];
                ValueObjectStatusEnum defaultStatus = ValueObjectStatusEnum.NotState;

                // DataSetDimension
                DataSetDimension[] dataSetDimension = new DataSetDimension[]
                {
                    new DataSetDimension() { DataSet = dataSet, Dimension = dimensions[0], ColumnName = "Dimension1", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new DataSetDimension() { DataSet = dataSet, Dimension = dimensions[1], ColumnName = "Dimension2", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                foreach (DataSetDimension s in dataSetDimension)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.DataSetDimension.Add(s);
                    context.SaveChanges();
                }

                // ValueObject
                ValueObject[] valueObjects = new ValueObject[]
                {
                    new ValueObject { DataSet = dataSet, Dimension1 = ".", Dimension2 = "Primelex", InitialValue = 100000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.", Dimension2 = "Primelex", InitialValue = 60000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T1.", Dimension2 = "Primelex", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T2.", Dimension2 = "Primelex", InitialValue = 20000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T3.", Dimension2 = "Primelex", InitialValue = 20000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T4.", Dimension2 = "Primelex", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.", Dimension2 = "Primelex", InitialValue = 40000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T5.", Dimension2 = "Primelex", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T6.", Dimension2 = "Primelex", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T7.", Dimension2 = "Primelex", InitialValue = 5000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T8.", Dimension2 = "Primelex", InitialValue = 15000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".", Dimension2 = "Primanan", InitialValue = 100000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.", Dimension2 = "Primanan", InitialValue = 50000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T1.", Dimension2 = "Primanan", InitialValue = 20000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T2.", Dimension2 = "Primanan", InitialValue = 25000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T3.", Dimension2 = "Primanan", InitialValue = 5000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R1.T4.", Dimension2 = "Primanan", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.", Dimension2 = "Primanan", InitialValue = 50000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T5.", Dimension2 = "Primanan", InitialValue = 25000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T6.", Dimension2 = "Primanan", InitialValue = 10000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T7.", Dimension2 = "Primanan", InitialValue = 5000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                    new ValueObject { DataSet = dataSet, Dimension1 = ".R2.T8.", Dimension2 = "Primanan", InitialValue = 20000, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now, Status = defaultStatus },
                };

                foreach (ValueObject s in valueObjects)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.ValueObject.Add(s);
                    context.SaveChanges();
                }

                // WorkflowConfig
                WorkflowConfig[] workflowConfig = new WorkflowConfig[]
                {
                    new WorkflowConfig() { Name = "WorkflowConfig1", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                foreach (WorkflowConfig s in workflowConfig)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.WorkflowConfig.Add(s);
                    context.SaveChanges();
                }

                // SelectorConfig
                SelectorConfig[] selectorConfigs = new SelectorConfig[]
                {
                    new SelectorConfig() { Name = "SelectorConfig_NationalModifiesRegions", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new SelectorConfig() { Name = "SelectorConfig_RegionModifiesTerritories", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                IEnumerable<SelectorConfig> selectorConfigReverse = selectorConfigs.Reverse();

                foreach (SelectorConfig s in selectorConfigReverse)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.SelectorConfig.Add(s);
                    context.SaveChanges();
                }

                // - PrevPropagate and Propagate part.

                //                                  SC1
                //                                +  |  +
                //                             +     |     +
                //                          +        |        +
                //                      SC2 >>>>>>  SC3 >>>>>>  SC4
                //                    +  |           |   +          +         
                //                 +     |           |      +          +
                //              +        |           |         +          +
                //          SC5 >>>>>>  SC6 >>>>>>  SC7 >>>>>>  SC8 >>>>>> SC9         

                // Légende :
                //      >>>>>> (PrevPropagate)
                //      + + + (Propagate)
                //      | (Propagate)

                // For updating all the SelectorConfig in this data, we have to follow a logic of deployment. 
                // PrevPropagate permits to propagate a call to another config at the same level.
                // Propagate permits to propagate a call the first config at the level next.

                // Example : 
                //      SC1 propagate to  SC2 ; 
                //      SC2 prevpropagate to SC3 and propagate to SC5 ;
                //      SC3 prevpropagate to SC4 and propagate to SC7 ; 
                //      SC4 prevpropagate to nothing and propagate to SC9 ;
                //      SC5 prevpropagate to SC6 and propagate to nothing ;
                //      (...)

                selectorConfigs[0].PrevPropagateId = 0;
                selectorConfigs[0].PropagateId = selectorConfigs[1].Id;
                selectorConfigs[1].PrevPropagateId = selectorConfigs[0].Id;
                selectorConfigs[1].PropagateId = 0;

                foreach (SelectorConfig s in selectorConfigs)
                {
                    context.Entry(s).State = EntityState.Modified;
                    context.SelectorConfig.Update(s);
                    context.SaveChanges();
                }

                // Criterias
                Criteria[] criterias = new Criteria[]
                {
                    // on va avoir x valeur du niveau spécifié et y valeur de produit => x * y instances
                    // level 0 de secto (donc normalement 1 valeur => x=1)
                    new Criteria { Value = "0", Dimension =  dimensions[0], SelectorConfig = selectorConfigs[0], Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    // dimension produit sans valeur => une instance par valeur contenue dans le dataset
                    // 2 produits => y = 2
                    new Criteria { Value = null, Dimension =  dimensions[1], SelectorConfig = selectorConfigs[0], Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    // pour 1 selectorConfigN, on aura dans notre exemple 2 selectorInstance générés au total

                    // level 1 => un selector instance pour ce niveau de secto (2 régions, x= 0)
                    new Criteria { Value = "1", Dimension =  dimensions[0], SelectorConfig = selectorConfigs[1], Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    // dimension produit sans valeur => une instance par valeur contenue dans le dataset
                    // 2 produits => y = 2
                    new Criteria { Value = null, Dimension =  dimensions[1], SelectorConfig = selectorConfigs[1], Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    // pour 1 selectorConfigR, on aura dans notre exemple 4 selectorInstance générés au total
                };

                foreach (Criteria s in criterias)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.Criteria.Add(s);
                    context.SaveChanges();
                }

                // WorkflowDimension
                WorkflowDimension[] workkflowDimension = new WorkflowDimension[]
                {
                    new WorkflowDimension() { WorkflowConfig = workflowConfig[0], Dimension = dimensions[0], ColumnName = "Dimension1", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new WorkflowDimension() { WorkflowConfig = workflowConfig[0], Dimension = dimensions[1], ColumnName = "Dimension2", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new WorkflowDimension() { WorkflowConfig = workflowConfig[0], Dimension = dimensions[0], ColumnName = "Dimension3", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new WorkflowDimension() { WorkflowConfig = workflowConfig[0], Dimension = dimensions[1], ColumnName = "Dimension4", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new WorkflowDimension() { WorkflowConfig = workflowConfig[0], Dimension = dimensions[0], ColumnName = "Dimension5", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },

                };

                foreach (WorkflowDimension s in workkflowDimension)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.WorkflowDimension.Add(s);
                    context.SaveChanges();
                }

                // User
                User[] user = new User[]
                {
                    new User() { Email = "user1@primeum.com", EmployeeID = "Employee1", Firstname = "Firstname1", Lastname ="Lastname1", Login = "login1", Password="password1", Name ="User1", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new User() { Email = "user2@primeum.com", EmployeeID = "Employee2", Firstname = "Firstname2", Lastname ="Lastname2", Login = "login2", Password="password2", Name ="User2", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new User() { Email = "user3@primeum.com", EmployeeID = "Employee3", Firstname = "Firstname3", Lastname ="Lastname3", Login = "login3", Password="password3", Name ="User3", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new User() { Email = "user4@primeum.com", EmployeeID = "Employee4", Firstname = "Firstname4", Lastname ="Lastname4", Login = "login4", Password="password4", Name ="User4", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                foreach (User s in user)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.Users.Add(s);
                    context.SaveChanges();
                }

                // UserSet
                UserSet[] userSet = new UserSet[]
                {
                    new UserSet() { Name = "Ensemble 1",  Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new UserSet() { Name = "Ensemble 2", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new UserSet() { Name = "Ensemble 3", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                };

                foreach (UserSet s in userSet)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.UserSet.Add(s);
                    context.SaveChanges();
                }

                // UserSetUser
                UserSetUser[] userSetUser = new UserSetUser[]
                {
                    new UserSetUser() { User = user[0], UserSet = userSet[0], Right = RightEnum.Consultation, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new UserSetUser() { User = user[1], UserSet = userSet[1], Right = RightEnum.Modification, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new UserSetUser() { User = user[2], UserSet = userSet[2], Right = RightEnum.Validation, Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                foreach (UserSetUser s in userSetUser)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.UserSetUser.Add(s);
                    context.SaveChanges();
                }

                // Comment
                Comment[] Comment = new Comment[]
                {
                    new Comment() { Author = user[0].Id, Receiver=user[1].Id, ValueObject = valueObjects[1], Date = DateTime.Now, Message = "Hello !", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new Comment() { Author = user[1].Id, Receiver=user[0].Id, ValueObject = valueObjects[1], Date = DateTime.Now, Message = "Hello !", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new Comment() { Author = user[0].Id, Receiver=user[1].Id, ValueObject = valueObjects[1], Date = DateTime.Now, Message = "How are you ?", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new Comment() { Author = user[1].Id, Receiver=user[0].Id, ValueObject = valueObjects[1], Date = DateTime.Now, Message = "Fine and you ?", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now },
                    new Comment() { Author = user[0].Id, Receiver=user[1].Id, ValueObject = valueObjects[1], Date = DateTime.Now, Message = "Badly...", Username = "MigrationUser", AddedDate = DateTime.Now, ModifiedDate = DateTime.Now }
                };

                foreach (Comment s in Comment)
                {
                    context.Entry(s).State = EntityState.Added;
                    context.Comment.Add(s);
                    context.SaveChanges();
                }
                */
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);

                return;
            }
        }
    }
}
