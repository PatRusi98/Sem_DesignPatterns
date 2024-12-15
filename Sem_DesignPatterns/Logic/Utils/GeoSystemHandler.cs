using Sem_DesignPatterns.Logic.Objects;
using Sem_DesignPatterns.Logic.Struct;
using System.IO;
using static Sem_DesignPatterns.Logic.Utils.Enums;

namespace Sem_DesignPatterns.Logic.Utils
{
    public class GeoSystemHandler
    {
        private KDTreeFactory _kdTreeFactory;
        private TreeManager<NodeData<Parcel>> _parcelTreeManager;
        private TreeManager<NodeData<Property>> _propertyTreeManager;
        private TreeManager<NodeData<GeoEntity>> _objectTreeManager;

        private static GeoSystemHandler? _instance = null;
        private Random _random = new();
        private Generator _generator = Generator.Instance;

        private GeoSystemHandler()
        {
            _kdTreeFactory = KDTreeFactory.Instance();
            _parcelTreeManager = new(_kdTreeFactory);
            _propertyTreeManager = new(_kdTreeFactory);
            _objectTreeManager = new(_kdTreeFactory);
        }

        public static GeoSystemHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GeoSystemHandler();
                }
                return _instance;
            }
        }

        public bool Insert(GeoEntity entity)
        {
            var success = false;
            if (entity is Parcel parcel)
            {
                success = _parcelTreeManager.Add(new NodeData<Parcel>(parcel.Key1) { Value = parcel });
                success = _parcelTreeManager.Add(new NodeData<Parcel>(parcel.Key2) { Value = parcel });

                success = _objectTreeManager.Add(new NodeData<GeoEntity>(parcel.Key1) { Value = parcel });
                success = _objectTreeManager.Add(new NodeData<GeoEntity>(parcel.Key2) { Value = parcel });

                var groupedItems = SearchOverlapItems(parcel).GroupBy(x => x.ID);
                List<GeoEntity> overlapItems = new();
                foreach (var group in groupedItems)
                {
                    overlapItems.Add(group.First());
                }
                parcel.AddSubAreas(overlapItems);

                List<GeoEntity> toAdd = new();
                toAdd.Add(parcel);

                foreach (var item in overlapItems)
                {
                    item.AddSubAreas(toAdd);
                }
            }
            else if (entity is Property property)
            {
                success = _propertyTreeManager.Add(new NodeData<Property>(property.Key1) { Value = property });
                success = _propertyTreeManager.Add(new NodeData<Property>(property.Key2) { Value = property });

                success = _objectTreeManager.Add(new NodeData<GeoEntity>(property.Key1) { Value = property });
                success = _objectTreeManager.Add(new NodeData<GeoEntity>(property.Key2) { Value = property });

                var groupedItems = SearchOverlapItems(property).GroupBy(x => x.ID);
                List<GeoEntity> overlapItems = new();
                foreach (var group in groupedItems)
                {
                    overlapItems.Add(group.First());
                }
                property.AddSubAreas(overlapItems);

                List<GeoEntity> toAdd = new();
                toAdd.Add(property);

                foreach (var item in overlapItems)
                {
                    item.AddSubAreas(toAdd);
                }
            }

            return success;
        }

        public bool Update(GeoEntity entityToEdit, GeoEntityParams par)
        {
            var success = false;

            if (par.Number != null)
            {
                entityToEdit.Number = par.Number.Value;
                success = true;
            }

            if (par.Description != null)
            {
                entityToEdit.Description = par.Description;
                success = true;
            }

            if (par.Point1 != null || par.Point2 != null)
            {
                entityToEdit.Point1 = par.Point1 ?? entityToEdit.Point1;
                entityToEdit.Point2 = par.Point2 ?? entityToEdit.Point2;

                entityToEdit.SubAreas = new();
                entityToEdit.AddSubAreas(SearchOverlapItems(entityToEdit));

                success = true;
                // TODO: vymazat a znovu pridat do stromu
            }

            return success;
        }

        public bool Delete(GeoEntity entity)
        {
            var success = false;
            if (entity is Parcel parcel)
            {
                var toDeleteSubAreaProp = _propertyTreeManager.Find(new NodeData<Property>(parcel.Key2));
                foreach (var item in toDeleteSubAreaProp)
                {
                    if (item.Value != null)
                    {
                        item.Value.SubAreas.Remove(parcel);
                    }
                }

                var toDeleteSubAreaAll = _objectTreeManager.Find(new NodeData<GeoEntity>(parcel.Key2));
                foreach (var item in toDeleteSubAreaAll)
                {
                    if (item.Value != null)
                    {
                        item.Value.SubAreas.Remove(parcel);
                    }
                }

                success = _parcelTreeManager.Remove(new NodeData<Parcel>(parcel.Key1) { Value = parcel });
                success = _parcelTreeManager.Remove(new NodeData<Parcel>(parcel.Key2) { Value = parcel });

                success = _objectTreeManager.Remove(new NodeData<GeoEntity>(parcel.Key1) { Value = parcel });
                success = _objectTreeManager.Remove(new NodeData<GeoEntity>(parcel.Key2) { Value = parcel });
            }
            else if (entity is Property property)
            {
                var toDeleteSubAreaParc = _propertyTreeManager.Find(new NodeData<Property>(property.Key1));
                foreach (var item in toDeleteSubAreaParc)
                {
                    if (item.Value != null)
                    {
                        item.Value.SubAreas.Remove(property);
                    }
                }

                var toDeleteSubAreaAll = _objectTreeManager.Find(new NodeData<GeoEntity>(property.Key1));
                foreach (var item in toDeleteSubAreaAll)
                {
                    if (item.Value != null)
                    {
                        item.Value.SubAreas.Remove(property);
                    }
                }

                success = _propertyTreeManager.Remove(new NodeData<Property>(property.Key1) { Value = property });
                success = _propertyTreeManager.Remove(new NodeData<Property>(property.Key2) { Value = property });

                success = _objectTreeManager.Remove(new NodeData<GeoEntity>(property.Key1) { Value = property });
                success = _objectTreeManager.Remove(new NodeData<GeoEntity>(property.Key2) { Value = property });
            }

            return success;
        }

        public List<GeoEntity> Search(GPSLocation gps, GeoEntityType type = GeoEntityType.Unknown)
        {
            var returnedNodes = new List<NodeData<GeoEntity>>();
            var result = new List<GeoEntity>();

            if (type == GeoEntityType.Parcel)
            {
                var searchResult = _parcelTreeManager.Find(new NodeData<Parcel>(gps.GPSToDouble()));
                if (searchResult != null)
                {
                    returnedNodes.AddRange(searchResult.Select(node => new NodeData<GeoEntity>(node.KeyArr) { Value = node.Value as GeoEntity }));
                }
            }
            else if (type == GeoEntityType.Property)
            {
                var searchResult = _propertyTreeManager.Find(new NodeData<Property>(gps.GPSToDouble()));
                if (searchResult != null)
                {
                    returnedNodes.AddRange(searchResult.Select(node => new NodeData<GeoEntity>(node.KeyArr) { Value = node.Value as GeoEntity }));
                }
            }
            else
            {
                var searchResult = _objectTreeManager.Find(new NodeData<GeoEntity>(gps.GPSToDouble()));
                if (searchResult != null)
                {
                    returnedNodes.AddRange(searchResult);
                }
            }

            foreach (var node in returnedNodes)
            {
                if (node.Value != null)
                {
                    result.Add(node.Value);
                }
            }

            return result;
        }

        public List<GeoEntity> SearchAll(GeoEntityType type = GeoEntityType.Unknown)
        {
            var returnedNodes = new List<NodeData<GeoEntity>>();
            var result = new List<GeoEntity>();

            if (type == GeoEntityType.Parcel)
                returnedNodes.AddRange(_parcelTreeManager.FindAll()!.Cast<NodeData<GeoEntity>>());
            else if (type == GeoEntityType.Property)
                returnedNodes.AddRange(_propertyTreeManager.FindAll()!.Cast<NodeData<GeoEntity>>());
            else
                returnedNodes.AddRange(_objectTreeManager.FindAll()!);

            foreach (var node in returnedNodes)
            {
                if (node.Value != null && !result.Any(x => x.ID == node.Value.ID)) // len ak sa tam este nenachadza (kvoli tomu, ze ich vkladame 2x)
                    result.Add(node.Value);
            }

            return result;
        }

        public void SaveToFile()
        {
            var allItems = SearchAll(GeoEntityType.Unknown);

            using (StreamWriter writer = new StreamWriter("")) // TODO: path
            {
                foreach (var item in allItems)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }

        public void Test(int numberOfIterations, double insertProb, double searchProb, double deleteProb)
        {
            var combinedProb = insertProb + searchProb + deleteProb;
            double insert = insertProb / combinedProb;
            double search = searchProb / combinedProb;
            double delete = deleteProb / combinedProb;
            var searched = new List<GeoEntity>();

            for (int i = 0; i < numberOfIterations; i++)
            {
                var operation = _random.NextDouble();

                switch (operation)
                {
                    case var expression when operation < insert:
                        searched.Add(_generator.GenerateEntity());
                        break;
                    case var expression when operation < insert + search:
                        Search(searched[_random.Next(searched.Count)].Point1);
                        break;
                    case var expression when operation > search:
                        var toDelete = searched[_random.Next(searched.Count)];
                        var ok = Delete(toDelete);
                        if (ok)
                            searched.Remove(toDelete);
                        break;
                    default:
                        throw new ArgumentException("Something went wrong while testing");
                }
            }
        }

        #region private
        private List<GeoEntity> SearchOverlapItems(GeoEntity entity)
        {
            var overlapList = new List<GeoEntity>();

            if (entity is Parcel parcel)
            {
                overlapList.AddRange(Search(parcel.Point1, GeoEntityType.Property));
                overlapList.AddRange(Search(parcel.Point2, GeoEntityType.Property));
            }
            else if (entity is Property property)
            {
                overlapList.AddRange(Search(property.Point1, GeoEntityType.Parcel));
                overlapList.AddRange(Search(property.Point2, GeoEntityType.Parcel));
            }

            return overlapList;
        }
        #endregion
    }
}
